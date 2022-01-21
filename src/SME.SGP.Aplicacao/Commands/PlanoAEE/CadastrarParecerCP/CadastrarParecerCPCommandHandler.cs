using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class CadastrarParecerCPCommandHandler : IRequestHandler<CadastrarParecerCPCommand, bool>
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public CadastrarParecerCPCommandHandler(
            IRepositorioPlanoAEE repositorioPlanoAEE,
            IMediator mediator,
            IConfiguration configuration)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(CadastrarParecerCPCommand request, CancellationToken cancellationToken)
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

            await GerarPendenciaCEFAI(plano, turma);
        }

        private async Task GerarPendenciaCEFAI(PlanoAEE plano, Turma turma)
        {
            var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var hostAplicacao = configuration["UrlFrontEnd"];
            var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? "da criança" : "do estudante";

            var titulo = $"Plano AEE para validação - {plano.AlunoNome} ({plano.AlunoCodigo}) - {ueDre}";
            var descricao = $@"O Plano AEE {estudanteOuCrianca} {plano.AlunoNome} ({plano.AlunoCodigo}) da turma {turma.NomeComModalidade()} da {ueDre} foi cadastrado. <br/><a href='{hostAplicacao}aee/plano/editar/{plano.Id}'>Clique aqui</a> para acessar o plano e atribuir um PAAI para que ele registre o parecer.

                <br/><br/>A pendência será resolvida automaticamente após este registro.";

            await mediator.Send(new GerarPendenciaPlanoAEECommand(plano.Id, null, titulo, descricao, turma.UeId, PerfilUsuario.CEFAI));
        }

        private async Task<bool> ParametroGeracaoPendenciaAtivo()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarPendenciasPlanoAEE, DateTime.Today.Year));

            return parametro != null && parametro.Ativo;
        }
    }
}
