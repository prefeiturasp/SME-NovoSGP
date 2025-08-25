using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal : IRepositorioBase<PainelEducacionalRegistroFrequenciaAgrupamentoGlobal>
    {
        Task ExcluirFrequenciaGlobal(PainelEducacionalRegistroFrequenciaAgrupamentoGlobal entidade);
        Task<bool> SalvarFrequenciaGlobal(PainelEducacionalRegistroFrequenciaAgrupamentoGlobal entidade);
        Task<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoGlobal>> ObterFrequenciaGlobal(string codigoDre, string codigoUe);
    }
}
