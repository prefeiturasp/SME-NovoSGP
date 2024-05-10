﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirComunicadoCommandHandler : IRequestHandler<InserirComunicadoCommand, bool>
    {
        private readonly IRepositorioComunicado repositorioComunicado;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public InserirComunicadoCommandHandler(IRepositorioComunicado repositorioComunicado, IUnitOfWork unitOfWork, IMediator mediator)
        {
            this.repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(InserirComunicadoCommand request, CancellationToken cancellationToken)
        {
            var comunicado = new Comunicado();

            if (request.DataExpiracao.HasValue && request.DataExpiracao.Value < request.DataEnvio)
                throw new NegocioException("A data de expiração deve ser maior ou igual a data de envio.");


            MapearParaEntidade(request, comunicado);

            try
            {
                unitOfWork.IniciarTransacao();

                var id = await repositorioComunicado.SalvarAsync(comunicado);

                if (comunicado.Modalidades.NaoEhNulo())
                    await mediator.Send(new InserirComunicadoModalidadeCommand(comunicado));

                if (comunicado.TiposEscolas.NaoEhNulo())
                    await mediator.Send(new InserirComunicadoTipoEscolaCommand(comunicado));

                if (comunicado.Turmas.NaoEhNulo() && comunicado.Turmas.Any())
                {
                    if (comunicado.AnosEscolares.NaoEhNulo())
                        await mediator.Send(new InserirComunicadoAnoEscolarCommand(comunicado));

                    await InserirComunicadoTurma(comunicado);
                }

                if (comunicado.Alunos.NaoEhNulo() && comunicado.Alunos.Any())
                    await InserirComunicadoAluno(comunicado);

                await mediator.Send(new CriarNotificacaoEscolaAquiCommand(comunicado));

                unitOfWork.PersistirTransacao();

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao incluir comunicado: {ex}", LogNivel.Critico, LogContexto.Comunicado));
                unitOfWork.Rollback();
                throw;
            }
        }

        private async Task InserirComunicadoAluno(Comunicado comunicado)
        {
            comunicado.AtualizarIdAlunos();
            await mediator.Send(new InserirComunicadoAlunoCommand(comunicado.Alunos));
        }

        private async Task InserirComunicadoTurma(Comunicado comunicado)
        {
            comunicado.AtualizarIdTurmas();
            await mediator.Send(new InserirComunicadoTurmaCommand(comunicado.Turmas));
        }

        private void MapearParaEntidade(InserirComunicadoCommand request, Comunicado comunicado)
        {
            comunicado.DataEnvio = request.DataEnvio;
            comunicado.DataExpiracao = request.DataExpiracao;
            comunicado.AlunoEspecificado = request.AlunoEspecificado;
            comunicado.Descricao = request.Descricao;
            comunicado.Titulo = request.Titulo;
            comunicado.AnoLetivo = request.AnoLetivo;
            comunicado.SeriesResumidas = request.SeriesResumidas;
            comunicado.TipoCalendarioId = request.TipoCalendarioId;
            comunicado.EventoId = request.EventoId;


            if (!request.CodigoDre.Equals("-99"))
                comunicado.CodigoDre = request.CodigoDre;

            if (!request.CodigoUe.Equals("-99"))
                comunicado.CodigoUe = request.CodigoUe;

            if (request.Turmas.NaoEhNulo() && request.Turmas.Any() && !request.Turmas.Any(a => a == "-99"))
                request.Turmas.ToList().ForEach(x => comunicado.AdicionarTurma(x));

            if (request.Modalidades.Any() && !request.Modalidades.Any(a => a == -99))
                comunicado.Modalidades = request.Modalidades;

            if (request.TiposEscolas.Any() && !request.TiposEscolas.Any(a => a == -99))
                comunicado.TiposEscolas = request.TiposEscolas;

            if (request.AnosEscolares.Any() && !request.AnosEscolares.Any(a => a == "-99"))
                comunicado.AnosEscolares = request.AnosEscolares;

            if (request.AlunoEspecificado)
                request.Alunos.ToList().ForEach(x => comunicado.AdicionarAluno(x));

            if (request.Semestre > 0)
                comunicado.Semestre = request.Semestre;

            comunicado.SetarTipoComunicado();
        }
    }
}
