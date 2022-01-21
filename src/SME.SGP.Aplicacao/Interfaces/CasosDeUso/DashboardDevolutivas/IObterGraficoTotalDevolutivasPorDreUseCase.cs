using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterGraficoTotalDevolutivasPorDreUseCase
    {
        Task<IEnumerable<GraficoBaseDto>> Executar(FiltroTotalDevolutivasPorDreDto filtro);
    }
}
