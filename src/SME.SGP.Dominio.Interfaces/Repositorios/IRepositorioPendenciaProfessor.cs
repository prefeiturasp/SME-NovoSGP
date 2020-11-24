using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaProfessor
    {
        Task<long> Inserir(long pendenciaId, long turmaId, long componenteCurricularId, string professorRf);
        Task<bool> ExistePendenciaProfessorPorTurmaEComponente(long turmaId, long componenteCurricularId, string professorRf, TipoPendencia tipoPendencia);
        Task<long> ObterPendenciaIdPorTurma(long turmaId, TipoPendencia tipoPendencia);
        Task<IEnumerable<PendenciaProfessorDto>> ObterPendenciasPorPendenciaId(long pendenciaId);
    }
}
