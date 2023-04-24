using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioAnaliticoSondagemUseCase : IRelatorioAnaliticoSondagemUseCase
    {
        private readonly IMediator mediator;

        public RelatorioAnaliticoSondagemUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioAnaliticoSondagemDto filtroRelatorioAnaliticoSondagemDto)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            var perfil = await mediator.Send(new ObterPerfilAtualQuery());

            filtroRelatorioAnaliticoSondagemDto.PerfilUsuarioLogado = perfil;
            filtroRelatorioAnaliticoSondagemDto.LoginUsuarioLogado = usuarioLogado.Login;

            if (usuarioLogado == null)
                throw new NegocioException(MensagemNegocioComuns.NAO_FOI_POSSIVEL_LOCALIZAR_USUARIO);

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.RelatorioAnaliticoSondagem, filtroRelatorioAnaliticoSondagemDto, usuarioLogado, 
                                                                rotaRelatorio: (RelatorioTodasDresUes(filtroRelatorioAnaliticoSondagemDto.DreCodigo) ?
                                                                                    RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosAnaliticoSondagemTodasDresUes
                                                                                    : RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosAnaliticoSondagem)                                                               
                                                                ,formato:TipoFormatoRelatorio.Xlsx));
        }

        private bool RelatorioTodasDresUes(string codigoDre) => string.IsNullOrEmpty(codigoDre) || (codigoDre == "-99");
    }
}
