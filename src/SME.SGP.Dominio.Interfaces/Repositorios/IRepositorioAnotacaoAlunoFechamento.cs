using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoAluno : IRepositorioBase<FechamentoAluno>
    {
        Task<IEnumerable<FechamentoAlunoAnotacaoConselhoDto>> ObterAnotacoesTurmaAlunoBimestreAsync(string alunoCodigo, string turmaCodigo, int bimestre, bool EhFinal);

        Task<FechamentoAluno> ObterFechamentoAluno(long fechamentoTurmaDisciplinaId, string codigoAluno);

        Task<FechamentoAluno> ObterFechamentoAlunoENotas(long fechamentoTurmaDisciplinaId, string alunoCodigo);

        Task<IEnumerable<FechamentoAluno>> ObterPorFechamentoTurmaDisciplina(long fechamentoDisciplinaId);
    }
}