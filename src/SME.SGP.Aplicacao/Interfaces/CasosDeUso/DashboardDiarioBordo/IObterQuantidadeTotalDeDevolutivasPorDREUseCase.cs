using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterQuantidadeTotalDeDevolutivasPorDREUseCase
    {
        Task<IEnumerable<GraficoTotalDevolutivasPorAnoDTO>> Executar(FiltroDasboardDiarioBordoDevolutivasDto filtro);
    }
}
