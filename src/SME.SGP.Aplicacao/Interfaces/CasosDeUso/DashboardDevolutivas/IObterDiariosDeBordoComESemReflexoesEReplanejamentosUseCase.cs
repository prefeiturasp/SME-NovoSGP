using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterDiariosDeBordoComESemReflexoesEReplanejamentosUseCase
    {
        Task<IEnumerable<GraficoDiariosDeBordoComESemReflexoesEReplanejamentosDto>> Executar(FiltroGraficoDiariosDeBordoComESemReflexoesEReplanejamentosDto filtro);
    }
}