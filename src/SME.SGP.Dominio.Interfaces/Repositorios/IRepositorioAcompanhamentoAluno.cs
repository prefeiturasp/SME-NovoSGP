using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAcompanhamentoAluno : IRepositorioBase<AcompanhamentoAluno>
    {
        Task<AcompanhamentoAlunoSemestre> ObterAcompanhamentoPorTurmaAlunoESemestre(long turmaId, string alunoCodigo, int semestre);
        Task<long> ObterPorTurmaEAluno(long turmaId, string alunoCodigo);
        Task<int> ObterTotalAlunosComAcompanhamentoPorTurmaAnoLetivoESemestre(long turmaId, int anoLetivo, int semestre);

        Task<int> ObterTotalAlunosTurmaAnoLetivoESemestre(long turmaId, int anoLetivo, int semestre);
    }
}
