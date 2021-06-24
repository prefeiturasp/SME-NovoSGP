﻿using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class CadastrarDevolutivaCPCommandHandler : IRequestHandler<CadastrarDevolutivaCPCommand, bool>
    {

        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        public CadastrarDevolutivaCPCommandHandler(
            IRepositorioPlanoAEE repositorioPlanoAEE,
            IMediator mediator,
            IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(CadastrarDevolutivaCPCommand request, CancellationToken cancellationToken)
        {
            var planoAEE = await repositorioPlanoAEE.ObterPorIdAsync(request.PlanoAEEId);

            if (planoAEE == null)
                throw new NegocioException("Plano AEE não encontrado!");

            planoAEE.Situacao = Dominio.Enumerados.SituacaoPlanoAEE.AtribuicaoPAAI;
            planoAEE.ParecerCoordenacao = request.ParecerCoordenacao;

            var idEntidadeEncaminhamento = await repositorioPlanoAEE.SalvarAsync(planoAEE);

            await ExcluirPendenciaCPs(planoAEE);
            await GerarPendenciaCEFAI(planoAEE, planoAEE.TurmaId);

            return idEntidadeEncaminhamento != 0;
        }

        private async Task ExcluirPendenciaCPs(PlanoAEE planoAEE)
            => await mediator.Send(new ExcluirPendenciaPlanoAEECommand(planoAEE.Id));

        private async Task GerarPendenciaCEFAI(PlanoAEE plano, long turmaId)
        {
            if (!await ParametroGeracaoPendenciaAtivo())
                return;

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaId));
            if (turma == null)
                throw new NegocioException($"Não foi possível localizar a turma [{turmaId}]");

            var usuarioId = await mediator.Send(new ObtemUsuarioCEFAIDaDreQuery(turma.Ue.Dre.CodigoDre));
            if (usuarioId > 0)
                await GerarPendenciaCEFAI(usuarioId, plano, turma);
        }

        private async Task GerarPendenciaCEFAI(long usuarioId, PlanoAEE plano, Turma turma)
        {
            var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var hostAplicacao = configuration["UrlFrontEnd"];
            var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? "da criança" : "do estudante";

            var titulo = $"Plano AEE a encerrar - {plano.AlunoNome} ({plano.AlunoCodigo}) - {ueDre}";
            var descricao = $@"Foi solicitado o encerramento do Plano AEE {estudanteOuCrianca} {plano.AlunoNome} ({plano.AlunoCodigo}) da turma {turma.NomeComModalidade()} da {ueDre}. <br/><a href='{hostAplicacao}aee/plano/editar/{plano.Id}'>Clique aqui</a> para acessar o plano e atribuir um PAAI para analisar e realizar a devolutiva.
                <br/><br/>A pendência será resolvida automaticamente após este registro.";

            await mediator.Send(new GerarPendenciaPlanoAEECommand(plano.Id, usuarioId, titulo, descricao));
        }

        private async Task<bool> ParametroGeracaoPendenciaAtivo()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarPendenciasPlanoAEE, DateTime.Today.Year));

            return parametro != null && parametro.Ativo;
        }
    }
}
