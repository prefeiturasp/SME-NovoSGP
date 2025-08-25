using SME.SGP.Dominio.Entidades;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal : IRepositorioBase<PainelEducacionalRegistroFrequenciaAgrupamentoMensal>
    {
        Task ExcluirFrequenciaMensal(long frequenciaId);
        Task<bool> SalvarFrequenciaMensal(PainelEducacionalRegistroFrequenciaAgrupamentoMensal entidade);
    }
}
