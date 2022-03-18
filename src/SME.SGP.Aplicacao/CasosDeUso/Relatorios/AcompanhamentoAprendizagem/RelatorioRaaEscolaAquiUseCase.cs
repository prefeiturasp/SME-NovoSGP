using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioRaaEscolaAquiUseCase : IRelatorioRaaEscolaAquiUseCase
    {
        private readonly IMediator mediator;

        public RelatorioRaaEscolaAquiUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioEscolaAquiDto filtro)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioPorIdQuery(1));
            var turmaId = await mediator.Send(new ObterTurmaIdPorCodigoQuery(filtro.TurmaCodigo));
            filtro.TurmaId = turmaId;
            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.RaaEscolaAqui, filtro,
            usuarioLogado, formato: TipoFormatoRelatorio.Html,
            rotaRelatorio:  RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosRaaEscolaAqui, notificarErroUsuario: true));
        }
    }
}
