using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioAtendimentoNaapaDetalhadoUseCase : IRelatorioAtendimentoNaapaDetalhadoUseCase
    {
        private readonly IMediator mediator;

        public RelatorioAtendimentoNaapaDetalhadoUseCase(IMediator mediator)
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

            await mediator.Send(new RegistrarHistoricoDeAlteracaoDeImpressaoDoAtendimentoNAAPACommand(param.EncaminhamentoNaapaIds, usuarioLogado.Id));

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.RelatorioEncaminhamentoNaapaDetalhado,param,usuarioLogado,rotaRelatorio:RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosEncaminhamentoNaapaDetalhado));
        }
    }
}