using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAcompanhamentoAluno : IRepositorioBase<AcompanhamentoAluno>
    {
        Task<AcompanhamentoAlunoSemestre> ObterAcompanhamentoPorTurmaAlunoESemestre(long turmaId, string alunoCodigo, int semestre);
        Task<long> ObterPorTurmaEAluno(long turmaId, string alunoCodigo);
        Task<int> ObterTotalAlunosComAcompanhamentoPorTurmaSemestre(long turmaId, int semestre);

        Task<int> ObterTotalAlunosTurmaSemestre(long turmaId,  int semestre);
    }
}
