using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterFiltroRelatoriosModalidadesPorUeAbrangenciaUseCase
    {
        Task<IEnumerable<OpcaoDropdownDto>> Executar(string codigoUe, bool consideraNovasModalidades);
    }
}
