using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class RelatorioEncaminhamentoNaapaUseCase : IRelatorioEncaminhamentoNaapaUseCase
    {
        private readonly IMediator mediator;

        public RelatorioEncaminhamentoNaapaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioEncaminhamentoNaapaDto param)
        {
            //var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            var usuarioLogado = await mediator.Send(new ObterUsuarioPorRfQuery(param.UsuarioRf));
            param.UsuarioNome = usuarioLogado.Nome;
            param.UsuarioRf = usuarioLogado.CodigoRf;

            if (usuarioLogado == null)
                throw new NegocioException(MensagemNegocioComuns.NAO_FOI_POSSIVEL_LOCALIZAR_USUARIO);

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.RelatorioEncaminhamentoNaapa,param,usuarioLogado,rotaRelatorio:RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosEncaminhamentoNaapa));
        }
    }
}