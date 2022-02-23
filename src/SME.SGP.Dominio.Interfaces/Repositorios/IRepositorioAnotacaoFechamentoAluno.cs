using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAnotacaoFechamentoAluno : IRepositorioBase<AnotacaoFechamentoAluno>
    {
        Task<AnotacaoFechamentoAluno> ObterPorFechamentoEAluno(long fechamentoTurmaDisciplinaId, string alunoCodigo);
        Task<IEnumerable<FechamentoAlunoAnotacaoConselhoDto>> ObterAnotacoesTurmaAlunoBimestreAsync(string alunoCodigo, string[] turmasCodigos, long periodoId);
    }
}
