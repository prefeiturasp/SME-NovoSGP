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
        private readonly IUnitOfWork unitOfWork;

        public AtribuirResponsavelPlanoAEECommandHandler(IMediator mediator, IRepositorioPlanoAEE repositorioPlanoAEE, IConfiguration configuration,IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(AtribuirResponsavelPlanoAEECommand request, CancellationToken cancellationToken)
        {
            request.PlanoAEE.Situacao = request.PlanoAEE.ObterSituacaoAoAtribuirResponsavelPAAI();
            request.PlanoAEE.ResponsavelPaaiId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(request.ResponsavelRF));

            return await SalvarGerarPendenciaPaai(request, request.PlanoAEE);
        }

        private async Task<bool> SalvarGerarPendenciaPaai(AtribuirResponsavelPlanoAEECommand request, PlanoAEE planoAEE)
        {
            bool idEntidadeEncaminhamento = false;
            try
            {
                unitOfWork.IniciarTransacao();

                idEntidadeEncaminhamento = await repositorioPlanoAEE.SalvarAsync(planoAEE) > 0;
                await VerificaGeracaoPendenciaPAAI(planoAEE, request.Turma);

                unitOfWork.PersistirTransacao();
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                throw;
            }
            return idEntidadeEncaminhamento;
        }

        private async Task VerificaGeracaoPendenciaPAAI(PlanoAEE planoAEE, Turma turma)
        {
            if (planoAEE.EhSituacaoExpiradoValidado())
                return;

            await ExcluirPendenciaCEFAI(planoAEE);

            if (!await ParametroGeracaoPendenciaAtivo() || await AtribuidoAoMesmoUsuario(planoAEE))
                return;            

            await GerarPendenciaPAAI(planoAEE, turma);
        }

        private async Task<bool> AtribuidoAoMesmoUsuario(PlanoAEE planoAEE)
        {
            var usuarioId = await mediator.Send(ObterUsuarioLogadoIdQuery.Instance);
            return usuarioId == planoAEE.ResponsavelPaaiId;
        }

        private async Task GerarPendenciaPAAI(PlanoAEE plano, Turma turma)
        {
            var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var hostAplicacao = configuration["UrlFrontEnd"];
            var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? "da criança" : "do estudante";

            var titulo = $"Plano AEE para validação - {plano.AlunoNome} ({plano.AlunoCodigo}) - {ueDre}";
            var descricao = $@"O Plano AEE {estudanteOuCrianca} {plano.AlunoNome} ({plano.AlunoCodigo}) da turma {turma.NomeComModalidade()} da {ueDre} foi cadastrado. <br/><a href='{hostAplicacao}aee/plano/editar/{plano.Id}'>Clique aqui</a> para acessar o plano e registrar o seu parecer.
                <br/><br/>A pendência será resolvida automaticamente após este registro.";

            await mediator.Send(new GerarPendenciaPlanoAEECommand(plano.Id, plano.ResponsavelPaaiId.Value, titulo, descricao, turma.UeId, turma.Id));
        }

        private async Task ExcluirPendenciaCEFAI(PlanoAEE planoAEE)
        {
            await mediator.Send(new ExcluirPendenciaPlanoAEECommand(planoAEE.Id));
        }

        private async Task<bool> ParametroGeracaoPendenciaAtivo()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarPendenciasPlanoAEE, DateTime.Today.Year));

            return parametro.NaoEhNulo() && parametro.Ativo;
        }
    }
}
