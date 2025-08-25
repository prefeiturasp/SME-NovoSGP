using SME.SGP.Dominio.Entidades;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal : IRepositorioBase<PainelEducacionalRegistroFrequenciaAgrupamentoMensal>
    {
        Task ExcluirFrequenciaMensal(PainelEducacionalRegistroFrequenciaAgrupamentoMensal entidade);
        Task<bool> SalvarFrequenciaMensal(PainelEducacionalRegistroFrequenciaAgrupamentoMensal entidade);
    }
}
