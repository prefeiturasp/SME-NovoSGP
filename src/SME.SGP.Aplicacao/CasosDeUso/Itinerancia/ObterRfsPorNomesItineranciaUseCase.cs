using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRfsPorNomesItineranciaUseCase : IObterRfsPorNomesItineranciaUseCase
    {
        private readonly IMediator mediator;

        public ObterRfsPorNomesItineranciaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ItineranciaNomeRfCriadorRetornoDto>> Executar(string nomeParaBusca)
        {
            return await mediator.Send(new ObterItineranciasRfsCriadoresPorNomeQuery(nomeParaBusca));
        }
    }
}
