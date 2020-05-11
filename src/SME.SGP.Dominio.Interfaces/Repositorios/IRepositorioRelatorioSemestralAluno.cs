using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRelatorioSemestralAluno : IRepositorioBase<RelatorioSemestralAluno>
    {
        Task<RelatorioSemestralAluno> ObterPorTurmaAlunoAsync(long relatorioSemestralId, string alunoCodigo);
        Task<RelatorioSemestralAluno> ObterCompletoPorIdAsync(long relatorioSemestralAlunoId);
        Task<IEnumerable<RelatorioSemestralAluno>> ObterRelatoriosAlunosPorTurmaAsync(long turmaId, int semestre);
    }
}