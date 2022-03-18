using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioAcompanhamentoAprendizagemUseCase : AbstractUseCase,
        IRelatorioAcompanhamentoAprendizagemUseCase
    {
        public RelatorioAcompanhamentoAprendizagemUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(FiltroRelatorioAcompanhamentoAprendizagemDto filtro)
        {
            var usuarioLogado = filtro.TipoRelatorio != TipoRelatorio.RaaEscolaAqui ? await mediator.Send(new ObterUsuarioLogadoQuery()) 
                                                                                    : await mediator.Send(new ObterUsuarioPorIdQuery(1));
            var notificarUsuario = filtro.TipoRelatorio != TipoRelatorio.RaaEscolaAqui ? false : true;
            return await mediator.Send(new GerarRelatorioCommand(filtro.TipoRelatorio, filtro,
                usuarioLogado, formato: TipoFormatoRelatorio.Html,
                rotaRelatorio: filtro.TipoRelatorio != TipoRelatorio.RaaEscolaAqui ? RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosRelatorioAcompanhamentoAprendizagem 
                                                                                   : RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosRaaEscolaAqui, notificarErroUsuario: notificarUsuario));
        }
    }
}