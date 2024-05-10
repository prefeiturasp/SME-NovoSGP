using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalRIsPorDreUseCase : IObterTotalRIsPorDreUseCase
    {
        private readonly IMediator mediator;

        public ObterTotalRIsPorDreUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroDashboardTotalRIsPorDreDTO filtro)
            => await mediator.Send(new ObterTotalRIsPorDreQuery(filtro.AnoLetivo, filtro.Ano));
    }
}
