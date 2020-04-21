using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConselhoClasseAluno : IRepositorioBase<ConselhoClasseAluno>
    {
        Task<ConselhoClasseAluno> ObterPorFechamentoAsync(long fechamentoTurmaId, string alunoCodigo);
        Task<ConselhoClasseAluno> ObterPorFiltrosAsync(string codigoTurma, string codigoAluno, int bimestre, bool EhFinal);
        Task<ConselhoClasseAluno> ObterPorConselhoClasseAsync(long conselhoClasseId, string alunoCodigo);
        Task<ConselhoClasseAluno> ObterPorPeriodo(string alunoCodigo, long turmaId, long periodoEscolarId);
    }
}