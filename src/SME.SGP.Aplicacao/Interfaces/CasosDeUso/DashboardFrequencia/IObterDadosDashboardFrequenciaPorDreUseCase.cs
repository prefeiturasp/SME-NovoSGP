using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterDadosDashboardFrequenciaPorDreUseCase
    {
        Task<IEnumerable<GraficoFrequenciaGlobalPorDREDto>> Executar(FiltroGraficoFrequenciaGlobalPorDREDto filtro);
    }
}