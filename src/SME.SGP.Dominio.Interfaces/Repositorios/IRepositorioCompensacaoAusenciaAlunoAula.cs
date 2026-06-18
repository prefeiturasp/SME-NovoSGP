using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCompensacaoAusenciaAlunoAula : IRepositorioBase<CompensacaoAusenciaAlunoAula>
    {
        Task<bool> ExclusaoLogicaCompensacaoAusenciaAlunoAulaPorIds(long[] ids);
        Task<bool> ExcluirCompensacaoAusenciaPorCompensacaoAlunoId(long compensacaoAusenciaAlunoId);
    }
}
