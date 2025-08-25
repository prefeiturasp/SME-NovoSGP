using SME.SGP.Dominio.Entidades;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola : IRepositorioBase<PainelEducacionalRegistroFrequenciaAgrupamentoEscola>
    {
        Task ExcluirFrequenciaGlobal(PainelEducacionalRegistroFrequenciaAgrupamentoEscola entidade);
        Task<bool> SalvarFrequenciaGlobal(PainelEducacionalRegistroFrequenciaAgrupamentoEscola entidade);
    }
}
