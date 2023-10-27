using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao.Commands
{
    public class CadastrarParecerCPCommandHandler : IRequestHandler<CadastrarParecerCPCommand, bool>
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork unitOfWork;

        public CadastrarParecerCPCommandHandler(
            IRepositorioPlanoAEE repositorioPlanoAEE,
            IMediator mediator,
            IConfiguration configuration,
            IUnitOfWork unitOfWork)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(CadastrarParecerCPCommand request, CancellationToken cancellationToken)
        {
            var planoAEE = await repositorioPlanoAEE.ObterPorIdAsync(request.PlanoAEEId);

            if (planoAEE.EhNulo())
                throw new NegocioException(MensagemNegocioPlanoAee.Plano_aee_nao_encontrado);
            
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(planoAEE.TurmaId), cancellationToken);

            if (turma.EhNulo())
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            planoAEE.Situacao = Dominio.Enumerados.SituacaoPlanoAEE.AtribuicaoPAAI;
            planoAEE.ParecerCoordenacao = request.ParecerCoordenacao;

            var funcionarioPAAI = await mediator.Send(new ObterResponsavelAtribuidoUePorUeTipoQuery(turma.Ue.CodigoUe, TipoResponsavelAtribuicao.PAAI), cancellationToken);

            var idEntidadeEncaminhamento = funcionarioPAAI.NaoEhNulo() && funcionarioPAAI.Count() == 1
            ? await mediator.Send(new AtribuirResponsavelPlanoAEECommand(planoAEE, funcionarioPAAI.FirstOrDefault().CodigoRf, turma))
            : await ExcluirPendenciaCPsGerarPendenciaCEFAI(planoAEE);

            return idEntidadeEncaminhamento;
        }

        private async Task<bool> ExcluirPendenciaCPsGerarPendenciaCEFAI(PlanoAEE planoAEE)
        {
            bool idEntidadeEncaminhamento = false;
            try
            {
                unitOfWork.IniciarTransacao();

                idEntidadeEncaminhamento = await repositorioPlanoAEE.SalvarAsync(planoAEE) > 0;
                await ExcluirPendenciaCPs(planoAEE);
                await GerarPendenciaCEFAI(planoAEE, planoAEE.TurmaId);

                unitOfWork.PersistirTransacao();
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
            
            return idEntidadeEncaminhamento;
        }

        private async Task ExcluirPendenciaCPs(PlanoAEE planoAEE)
            => await mediator.Send(new ExcluirPendenciaPlanoAEECommand(planoAEE.Id));

        private async Task GerarPendenciaCEFAI(PlanoAEE plano, long turmaId)
        {
            if (!await ParametroGeracaoPendenciaAtivo())
                return;

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaId));
            if (turma.EhNulo())
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

            await mediator.Send(new GerarPendenciaPlanoAEECommand(plano.Id, null, titulo, descricao, turma.UeId, turma.Id, PerfilUsuario.CEFAI));
        }

        private async Task<bool> ParametroGeracaoPendenciaAtivo()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarPendenciasPlanoAEE, DateTime.Today.Year));

            return parametro.NaoEhNulo() && parametro.Ativo;
        }
    }
}
