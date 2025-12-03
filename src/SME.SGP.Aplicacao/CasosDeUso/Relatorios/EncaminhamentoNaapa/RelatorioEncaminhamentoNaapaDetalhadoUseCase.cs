using System;
using System.Threading.Tasks;
using MediatR;
using Org.BouncyCastle.Crypto;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class RelatorioEncaminhamentoNaapaDetalhadoUseCase : IRelatorioEncaminhamentoNaapaDetalhadoUseCase
    {
        private readonly IMediator mediator;

        public RelatorioEncaminhamentoNaapaDetalhadoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioAtendimentoNaapaDetalhadoDto param)
        {
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            param.UsuarioNome = usuarioLogado.Nome;
            param.UsuarioRf = usuarioLogado.CodigoRf;

            if (usuarioLogado.EhNulo())
                throw new NegocioException(MensagemNegocioComuns.NAO_FOI_POSSIVEL_LOCALIZAR_USUARIO);

            await mediator.Send(new RegistrarHistoricoDeAlteracaoDeImpressaoDoEncaminhamentoNAAPACommand(param.EncaminhamentoNaapaIds, usuarioLogado.Id));

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.RelatorioEncaminhamentoNaapaDetalhado,param,usuarioLogado,rotaRelatorio:RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosEncaminhamentoNaapaDetalhado));
        }
    }
}