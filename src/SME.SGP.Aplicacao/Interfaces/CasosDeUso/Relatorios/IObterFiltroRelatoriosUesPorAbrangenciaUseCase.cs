using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterFiltroRelatoriosUesPorAbrangenciaUseCase
    {
        Task<IEnumerable<AbrangenciaUeRetorno>> Executar(string codigoDre);
    }
}