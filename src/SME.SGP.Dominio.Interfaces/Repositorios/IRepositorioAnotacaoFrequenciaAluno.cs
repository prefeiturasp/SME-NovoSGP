using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAnotacaoFrequenciaAluno : IRepositorioBase<AnotacaoFrequenciaAluno>
    {
        Task<bool> ExcluirAnotacoesDaAula(long aulaId);
    }
}