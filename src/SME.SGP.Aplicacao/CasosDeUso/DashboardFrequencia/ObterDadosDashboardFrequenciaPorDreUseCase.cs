using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardFrequenciaPorDreUseCase : IObterDadosDashboardFrequenciaPorDreUseCase
    {
        private readonly IMediator mediator;

        public ObterDadosDashboardFrequenciaPorDreUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<GraficoFrequenciaGlobalPorDREDto>> Executar(FiltroGraficoFrequenciaGlobalPorDREDto filtro) 
            => await mediator.Send(new ObterDadosDashboardFrequenciaPorDreQuery(filtro.AnoLetivo, filtro.Modalidade, filtro.Ano, filtro.Semestre));
    }
}