using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosLetivosPAPUseCase : IObterAnosLetivosPAPUseCase
    {
        private readonly IMediator mediator;

        public ObterAnosLetivosPAPUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<ObterAnoLetivoPAPRetornoDto>> Executar()
        {
            return await mediator.Send(new ObterAnosLetivosPAPQuery() { AnoAtual = DateTime.Today.Year });
        }
    }
}
