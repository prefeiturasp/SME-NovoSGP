using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCompensacaoAusenciaAlunoAulaConsulta : IRepositorioBase<CompensacaoAusenciaAlunoAula>
    {
        Task<IEnumerable<CompensacaoAusenciaAlunoAulaDto>> ObterCompensacoesAusenciasAlunoEAulaPorAulaIdTurmaComponenteQuantidade(long aulaId, int quantidade);
        Task<IEnumerable<CompensacaoAusenciaAlunoAula>> ObterPorCompensacaoIdAsync(long compensacaoId);
        Task<IEnumerable<CompensacaoAusenciaAlunoAula>> ObterPorAulaIdAsync(long aulaId, long? numeroAula);
        Task<IEnumerable<CompensacaoAusenciaAlunoAula>> ObterPorRegistroFrequenciaAlunoIdsAsync(long[] registroFrequenciaAlunoIds);
    }
}
