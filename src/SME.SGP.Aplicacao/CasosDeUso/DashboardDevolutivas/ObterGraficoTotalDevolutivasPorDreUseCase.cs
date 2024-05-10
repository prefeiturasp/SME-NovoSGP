using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterGraficoTotalDevolutivasPorDreUseCase : IObterGraficoTotalDevolutivasPorDreUseCase
    {
        private readonly IMediator mediator;

        public ObterGraficoTotalDevolutivasPorDreUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroTotalDevolutivasPorDreDto filtro)
            => await mediator.Send(new ObterTotalDevolutivasPorDreQuery(filtro.AnoLetivo, filtro.Ano));
    }
}
