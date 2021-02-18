using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosLetivosItineranciaUseCase : AbstractUseCase, IObterAnosLetivosItineranciaUseCase
    {
        public ObterAnosLetivosItineranciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<long>> Executar()
                => await mediator.Send(new ObterAnosLetivosItineranciaQuery());
    }
}
