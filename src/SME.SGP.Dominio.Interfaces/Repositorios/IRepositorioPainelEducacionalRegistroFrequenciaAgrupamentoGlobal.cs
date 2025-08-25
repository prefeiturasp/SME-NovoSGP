using SME.SGP.Dominio.Entidades;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal : IRepositorioBase<PainelEducacionalRegistroFrequenciaAgrupamentoGlobal>
    {
        Task ExcluirFrequenciaGlobal(long frequenciaId);
        Task<bool> SalvarFrequenciaGlobal(PainelEducacionalRegistroFrequenciaAgrupamentoGlobal entidade);
    }
}
