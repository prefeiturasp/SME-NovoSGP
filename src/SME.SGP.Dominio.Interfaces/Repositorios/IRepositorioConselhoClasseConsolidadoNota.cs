using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioConselhoClasseConsolidadoNota 
    {
        Task<ConselhoClasseConsolidadoTurmaAlunoNota> ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoNotaAsync(long consolidadoTurmaAlunoId, int bimestre, long? componenteCurricularId);
        Task<long> SalvarAsync(ConselhoClasseConsolidadoTurmaAlunoNota consolidadoNota);
    }
}
