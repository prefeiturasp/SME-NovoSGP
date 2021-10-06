using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioComponenteCurricular
    {
        Task<IEnumerable<ComponenteCurricularDto>> ListarComponentesCurriculares();
        Task<bool> VerificarComponenteCurriculareSeERegenciaPorId(long id);
        Task<IEnumerable<DisciplinaDto>> ObterComponentesCurricularesRegenciaPorAnoETurno(long ano, long turno);
        void SalvarVarias(IEnumerable<ComponenteCurricularDto> componentesCurriculares);
        Task<IEnumerable<DisciplinaDto>> ObterDisciplinasPorIds(long[] ids);
        Task<long[]> ListarCodigosJuremaPorComponenteCurricularId(long id);
        Task<bool> VerificaPossuiObjetivosAprendizagemPorComponenteCurricularId(long id);
        Task<bool> LancaNota(long id);
        Task<IEnumerable<ComponenteCurricularDto>> ObterComponentesComNotaDeFechamentoOuConselhoPorAlunoEBimestre(int anoLetivo, long turmaId, int bimestre, string codigoAluno);
    }
}