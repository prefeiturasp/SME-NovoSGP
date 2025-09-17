using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal
    {
        Task ExcluirFrequenciaGlobal(int anoLetivo);
        Task<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoGlobal>> ObterFrequenciaGlobal(int anoLetivo, string codigoDre, string codigoUe);
        Task BulkInsertAsync(IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoGlobal> registros);
    }
}
