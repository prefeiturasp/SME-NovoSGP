using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRelatorioSemestralPAPAluno : IRepositorioBase<RelatorioSemestralPAPAluno>
    {
        Task<RelatorioSemestralPAPAluno> ObterPorTurmaAlunoAsync(long relatorioSemestralId, string alunoCodigo);
        Task<RelatorioSemestralPAPAluno> ObterCompletoPorIdAsync(long relatorioSemestralAlunoId);
        Task<RelatorioSemestralPAPAluno> ObterRelatorioSemestralPorAlunoTurmaSemestreAsync(string alunoCodigo, string turmaCodigo, int semestre); 
        Task<IEnumerable<RelatorioSemestralAlunoSecaoDto>> ObterDadosSecaoPorRelatorioSemestralAlunoIdDataReferenciaAsync(long relatorioSemestralAlunoId, DateTime dataReferencia);
        Task<IEnumerable<RelatorioSemestralPAPAluno>> ObterRelatoriosAlunosPorTurmaAsync(long turmaId, int semestre);
    }
}