using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterGraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteUseCase
    {
        Task<IEnumerable<GraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteDto>> Executar(FiltroGraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteDto filtro);
    }
}