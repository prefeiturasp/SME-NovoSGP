using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConselhoClasseAluno : IRepositorioBase<ConselhoClasseAluno>
    {
        Task<ConselhoClasseAluno> ObterPorFiltrosAsync(string codigoTurma, string codigoAluno, int bimestre, bool EhFinal);
        Task<ConselhoClasseAluno> ObterPorConselhoClasseAsync(long conselhoClasseId, string alunoCodigo);
    }
}