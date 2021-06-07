using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCompensacaoAusenciaAluno : IRepositorioBase<CompensacaoAusenciaAluno>
    {
        Task<IEnumerable<CompensacaoAusenciaAluno>> ObterPorCompensacao(long compensacaoId);
        int ObterTotalCompensacoesPorAlunoETurma(int bimestre, string codigoAluno, string disciplinaId, string turmaId);
        Task<int> ObterTotalCompensacoesPorAlunoETurmaAsync(int bimestre, string codigoAluno, string disciplinaId, string turmaId);
        Task<IEnumerable<CompensacaoAusenciaAluno>> ObterCompensacoesAluno(string codigoAluno, long compensacaoIgnorada, int bimestre);
        Task<IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto>> ObterTotalCompensacoesPorAlunosETurmaAsync(IEnumerable<int> bimestres, IEnumerable<string> alunos, string turmaId);
    }
}
