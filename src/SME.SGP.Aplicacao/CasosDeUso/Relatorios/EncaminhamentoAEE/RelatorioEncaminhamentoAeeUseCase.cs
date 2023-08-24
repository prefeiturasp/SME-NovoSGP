using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioEncaminhamentoAeeUseCase : IRelatorioEncaminhamentoAEEUseCase
    {
        private readonly IMediator mediator;

        public RelatorioEncaminhamentoAeeUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioEncaminhamentoAEEDto param)
        {
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            param.UsuarioNome = usuarioLogado.Nome;
            param.UsuarioRf = usuarioLogado.CodigoRf;

            if (usuarioLogado == null)
                throw new NegocioException(MensagemNegocioComuns.NAO_FOI_POSSIVEL_LOCALIZAR_USUARIO);

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.RelatorioEncaminhamentosAee, param, usuarioLogado, rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosEncaminhamentoAee));
        }
    }
}
