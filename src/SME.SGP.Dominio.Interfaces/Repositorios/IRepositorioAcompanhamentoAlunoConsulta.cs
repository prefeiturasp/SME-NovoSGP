using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAcompanhamentoAlunoConsulta : IRepositorioBase<AcompanhamentoAluno>
    {
        Task<AcompanhamentoAlunoSemestre> ObterAcompanhamentoPorTurmaAlunoESemestre(long turmaId, string alunoCodigo, int semestre);
        Task<long> ObterPorTurmaEAluno(long turmaId, string alunoCodigo);
        Task<int> ObterTotalAlunosComAcompanhamentoPorTurmaSemestre(long turmaId, int semestre, string[] codigosAlunos);
        Task<int> ObterTotalAlunosTurmaSemestre(long turmaId,  int semestre);
        Task<int> ObterUltimoSemestreAcompanhamentoGerado(string alunoCodigo);
        Task<IEnumerable<AcompanhamentoAluno>> ObterAlunosQueContemPercursoIndividalPreenchido(long turmaId, int semestre);
    }
}
