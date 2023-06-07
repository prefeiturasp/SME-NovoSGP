using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAnotacaoFechamentoAlunoConsulta : IRepositorioBase<AnotacaoFechamentoAluno>
    {
        Task<AnotacaoFechamentoAluno> ObterPorFechamentoEAluno(long fechamentoTurmaDisciplinaId, string alunoCodigo);
        Task<IEnumerable<AnotacaoFechamentoAluno>> ObterPorFechamentoEAluno(long[] fechamentosTurmasDisciplinasIds, string[] alunosCodigos);
        Task<IEnumerable<FechamentoAlunoAnotacaoConselhoDto>> ObterAnotacoesTurmaAlunoBimestreAsync(string alunoCodigo, string[] turmasCodigos, long periodoId);
        Task<IEnumerable<string>> ObterAlunosComAnotacaoNoFechamento(long fechamentoTurmaDisciplinaId);
    }
}
