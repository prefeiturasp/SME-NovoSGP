using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarEventosPorCalendarioUseCase : AbstractUseCase, IListarEventosPorCalendarioUseCase
    {
        public ListarEventosPorCalendarioUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<EventoCalendarioRetornoDto>> Executar(ListarEventoPorCalendarioDto param)
            => await mediator.Send(new ListarEventosPorCalendarioQuery(param.TipoCalendario,
                                                                           param.AnoLetivo,
                                                                           param.CodigoDre,
                                                                           param.CodigoUe,
                                                                           param.Modalidades));

    }
}
