using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola
    {
        Task ExcluirFrequenciaGlobal(int anoLetivo);
        Task<bool> SalvarFrequenciaGlobal(IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoEscola> consolidacoes);
        Task<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoEscola>> ObterFrequenciaGlobal(int anoLetivo, string codigoDre, string codigoUe);
        Task BulkInsertAsync(IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoEscola> registros);
    }
}
