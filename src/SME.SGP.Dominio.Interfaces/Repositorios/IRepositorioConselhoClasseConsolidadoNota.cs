using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioConselhoClasseConsolidadoNota 
    {
        Task<ConselhoClasseConsolidadoTurmaAlunoNota> ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoNotaAsync(long consolidadoTurmaAlunoId, int? bimestre, long? componenteCurricularId);
        Task<long> SalvarAsync(ConselhoClasseConsolidadoTurmaAlunoNota consolidadoNota);
        Task<ConselhoClasseConsolidadoTurmaAlunoNota> ObterConselhoClasseConsolidadoAlunoNotaPorConsolidadoBimestreDisciplinaAsync(long consolidacaoId, int bimestre, long disciplinaId);
        Task<bool> ExcluirConsolidacaoConselhoClasseNotaPorIdsConsolidacaoAlunoEBimestre(long[] idsConsolidacao);
        Task<IEnumerable<long>> ObterConsolidacoesConselhoClasseNotaIdsPorConsolidacoesAlunoTurmaIds(long[] consolidacoesAlunoTurmaIds, int bimestre = 0);
    }
}
