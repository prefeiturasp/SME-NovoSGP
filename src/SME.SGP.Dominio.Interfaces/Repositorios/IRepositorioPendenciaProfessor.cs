using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaProfessor
    {
        Task<long> Inserir(long pendenciaId, long turmaId, long componenteCurricularId, string professorRf, long? periodoEscolarId);
        Task<bool> ExistePendenciaProfessorPorTurmaEComponente(long turmaId, long componenteCurricularId, long? periodoEscolarId, string professorRf, TipoPendencia tipoPendencia);
        Task<long> ObterPendenciaIdPorTurma(long turmaId, TipoPendencia tipoPendencia);
        Task<IEnumerable<PendenciaProfessorDto>> ObterPendenciasPorPendenciaId(long pendenciaId);
        Task<IEnumerable<PendenciaProfessor>> ObterPendenciasProfessorPorTurmaEComponente(long turmaId, long[] componentesCurriculares, long? periodoEscolarId, TipoPendencia tipoPendencia);
        Task<Turma> ObterTurmaDaPendencia(long pendenciaId);
        Task Remover(PendenciaProfessor pendenciaProfessor);
        Task<long> ObterPendenciaIdPorTurmaCCPeriodoEscolar(long turmaId, long componenteCurricularId, long periodoEscolarId, TipoPendencia tipoPendencia);
        Task<bool> ExistePendenciaProfessorPorPendenciaId(long pendenciaId);
    }
}
