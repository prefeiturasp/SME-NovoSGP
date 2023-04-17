using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCompensacaoAusenciaAlunoConsulta : IRepositorioBase<CompensacaoAusenciaAluno>
    {
        Task<IEnumerable<CompensacaoAusenciaAluno>> ObterPorCompensacao(long compensacaoId);        
        Task<int> ObterTotalCompensacoesPorAlunoETurmaAsync(int bimestre, string codigoAluno, string disciplinaId, string turmaId);
        Task<TotalCompensacaoAlunoPorCompensacaoIdDto> ObterTotalCompensacoesECompensacaoIdPorAlunoETurmaAsync(int bimestre, string codigoAluno, string disciplinaId, string turmaId);
        Task<IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto>> ObterTotalCompensacoesPorAlunosETurmaAsync(int bimestre, List<string> alunos, string turmaCodigo);
        Task<IEnumerable<CompensacaoAusenciaAlunoEDataDto>> ObterCompensacaoAusenciaAlunoEAulaPorAulaId(long aulaId);
        Task<IEnumerable<CompensacaoAusenciaAlunoEDataDto>> ObterCompensacoesAusenciasAlunosPorRegistroFrequenciaAlunoIdsQuery(IEnumerable<long> registroFrequenciaAlunoIds);
        Task<IEnumerable<CompensacaoAusenciaAluno>> ObterPorIdsAsync(long[] ids);
    }
}
