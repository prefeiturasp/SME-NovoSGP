using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtribuirResponsavelPlanoAEECommandHandler : IRequestHandler<AtribuirResponsavelPlanoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;
        private readonly IConfiguration configuration;

        public AtribuirResponsavelPlanoAEECommandHandler(IMediator mediator, IRepositorioPlanoAEE repositorioPlanoAEE, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(AtribuirResponsavelPlanoAEECommand request, CancellationToken cancellationToken)
        {
            var planoAEE = await mediator.Send(new ObterPlanoAEEPorIdQuery(request.PlanoAEEId));

            if (planoAEE == null)
                throw new NegocioException("O Plano AEE informado não foi encontrado");

            if (planoAEE.Situacao == Dominio.Enumerados.SituacaoPlanoAEE.Encerrado)
                throw new NegocioException("A situação do Plano AEE não permite a remoção do responsável");

            planoAEE.Situacao = Dominio.Enumerados.SituacaoPlanoAEE.ParecerPAAI;
            planoAEE.ResponsavelId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(request.ResponsavelRF));

            var idEntidadePlanoAEE = await repositorioPlanoAEE.SalvarAsync(planoAEE);

            await VerificaGeracaoPendenciaPAAI(planoAEE);

            return idEntidadePlanoAEE != 0;
        }

        private async Task VerificaGeracaoPendenciaPAAI(PlanoAEE planoAEE)
        {
            await ExcluirPendenciaCEFAI(planoAEE);

            if (!await ParametroGeracaoPendenciaAtivo() || await AtribuidoAoMesmoUsuario(planoAEE))
                return;

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(planoAEE.TurmaId));
            if (turma == null)
                throw new NegocioException($"Não foi possível localizar a turma [{planoAEE.TurmaId}]");

            await GerarPendenciaPAAI(planoAEE, turma);
        }

        private async Task<bool> AtribuidoAoMesmoUsuario(PlanoAEE planoAEE)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioLogadoIdQuery());
            return usuarioId == planoAEE.ResponsavelId;
        }

        private async Task GerarPendenciaPAAI(PlanoAEE plano, Turma turma)
        {
            var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var hostAplicacao = configuration["UrlFrontEnd"];
            var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? "da criança" : "do estudante";

            var titulo = $"Plano AEE para validação - {plano.AlunoNome} ({plano.AlunoCodigo}) - {ueDre}";
            var descricao = $@"O Plano AEE {estudanteOuCrianca} {plano.AlunoNome} ({plano.AlunoCodigo}) da turma {turma.NomeComModalidade()} da {ueDre} foi cadastrado. <br/><a href='{hostAplicacao}aee/plano/editar/{plano.Id}'>Clique aqui</a> para acessar o plano e registrar o seu parecer.
                <br/><br/>A pendência será resolvida automaticamente após este registro.";

            await mediator.Send(new GerarPendenciaPlanoAEECommand(plano.Id, plano.ResponsavelId.Value, titulo, descricao));
        }

        private async Task ExcluirPendenciaCEFAI(PlanoAEE planoAEE)
        {
            await mediator.Send(new ExcluirPendenciaPlanoAEECommand(planoAEE.Id));
        }

        private async Task<bool> ParametroGeracaoPendenciaAtivo()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarPendenciasPlanoAEE, DateTime.Today.Year));

            return parametro != null && parametro.Ativo;
        }
    }
}
