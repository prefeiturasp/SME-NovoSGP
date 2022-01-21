using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterQuantidadeTotalDeDiariosPendentesPorDREUseCase
    {
        Task<IEnumerable<GraficoTotalDiariosEDevolutivasPorDreDTO>> Executar(int anoLetivo, string ano = "");
    }
}
