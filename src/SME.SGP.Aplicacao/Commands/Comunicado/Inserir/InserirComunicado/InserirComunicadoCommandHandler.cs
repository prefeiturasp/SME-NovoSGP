using MediatR;
using SME.SGP.Dominio;
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

                await mediator.Send(new InserirComunicadoModalidadeCommand(comunicado));

                if (comunicado.Turmas != null && comunicado.Turmas.Any())
                    await InserirComunicadoTurma(comunicado);

                if (comunicado.Alunos != null && comunicado.Alunos.Any())
                    await InserirComunicadoAluno(comunicado);

                await mediator.Send(new CriarNotificacaoEscolaAquiCommand(comunicado));

                unitOfWork.PersistirTransacao();

                return true;
            }
            catch
            {
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
            comunicado.AlunoEspecificado = request.AlunosEspecificados;
            comunicado.Descricao = request.Descricao;
            comunicado.Titulo = request.Titulo;
            comunicado.AnoLetivo = request.AnoLetivo;
            comunicado.SeriesResumidas = request.SeriesResumidas;
            comunicado.TipoCalendarioId = request.TipoCalendarioId;
            comunicado.EventoId = request.EventoId;

            if (!request.CodigoDre.Equals("todas"))
                comunicado.CodigoDre = request.CodigoDre;

            if (!request.CodigoUe.Equals("todas"))
                comunicado.CodigoUe = request.CodigoUe;

            if (request.Turmas != null && request.Turmas.Any())
                request.Turmas.ToList().ForEach(x => comunicado.AdicionarTurma(x));

            if (request.Modalidades.Any())
                comunicado.Modalidades = request.Modalidades;

            if (request.AlunosEspecificados)
                request.Alunos.ToList().ForEach(x => comunicado.AdicionarAluno(x));

            if (request.Semestre > 0)
                comunicado.Semestre = request.Semestre;

            comunicado.SetarTipoComunicado();
        }
    }
}
