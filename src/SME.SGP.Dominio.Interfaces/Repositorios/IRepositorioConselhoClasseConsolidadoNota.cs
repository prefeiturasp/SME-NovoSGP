using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioConselhoClasseConsolidadoNota 
    {
        Task<ConselhoClasseConsolidadoTurmaAlunoNota> ObterConselhoClasseConsolidadoAlunoNotaPorConsolidadoBimestreAsync(long consolidadoTurmaAlunoId, int bimestre);
        Task<long> SalvarAsync(ConselhoClasseConsolidadoTurmaAlunoNota consolidadoNota);
        Task<ConselhoClasseConsolidadoTurmaAlunoNota> ObterConselhoClasseConsolidadoAlunoNotaPorConsolidadoBimestreDisciplinaAsync(long consolidacaoId, int bimestre, long disciplinaId);
    }
}
