using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola : IRepositorioBase<PainelEducacionalRegistroFrequenciaAgrupamentoEscola>
    {
        Task ExcluirFrequenciaGlobal(PainelEducacionalRegistroFrequenciaAgrupamentoEscola entidade);
        Task<bool> SalvarFrequenciaGlobal(PainelEducacionalRegistroFrequenciaAgrupamentoEscola entidade);
        Task<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoEscola>> ObterFrequenciaGlobal(string codigoDre, string codigoUe);
    }
}
