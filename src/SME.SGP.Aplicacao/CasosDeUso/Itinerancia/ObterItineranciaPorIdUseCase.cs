using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterItineranciaPorIdUseCase : AbstractUseCase, IObterItineranciaPorIdUseCase
    {
        public ObterItineranciaPorIdUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ItineranciaDto> Executar(long id)
                => await mediator.Send(new ObterItineranciaPorIdQuery(id));
    }
}
