using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterFiltroRelatoriosModalidadesPorUeUseCase
    {
        Task<IEnumerable<OpcaoDropdownDto>> Executar(string codigoUe);
    }
}