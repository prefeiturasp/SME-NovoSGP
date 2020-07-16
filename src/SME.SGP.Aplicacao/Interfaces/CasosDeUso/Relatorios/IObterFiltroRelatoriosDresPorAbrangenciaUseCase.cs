using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterFiltroRelatoriosDresPorAbrangenciaUseCase
    {
        Task<IEnumerable<AbrangenciaDreRetorno>> Executar();
    }
}