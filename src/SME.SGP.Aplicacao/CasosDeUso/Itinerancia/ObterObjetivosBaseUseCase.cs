using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterObjetivosBaseUseCase : AbstractUseCase, IObterObjetivosBaseUseCase
    {
        public ObterObjetivosBaseUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<IEnumerable<ItineranciaObjetivosBaseDto>> Executar()
                => await mediator.Send(new ObterObjetivosBaseItineranciaQuery());
    }
}
