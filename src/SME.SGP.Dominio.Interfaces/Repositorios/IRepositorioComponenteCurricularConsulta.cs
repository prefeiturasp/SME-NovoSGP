using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioComponenteCurricularConsulta
    {
        Task<IEnumerable<ComponenteCurricularDto>> ListarComponentesCurriculares();
        Task<bool> VerificarComponenteCurriculareSeERegenciaPorId(long id);
        Task<IEnumerable<DisciplinaDto>> ObterComponentesCurricularesRegenciaPorAnoETurno(long ano, long turno);        
        Task<IEnumerable<DisciplinaDto>> ObterDisciplinasPorIds(long[] ids);
        Task<long[]> ListarCodigosJuremaPorComponenteCurricularId(long id);
        Task<bool> VerificaPossuiObjetivosAprendizagemPorComponenteCurricularId(long id);
        Task<bool> LancaNota(long id);
        Task<IEnumerable<ComponenteCurricularDto>> ObterComponentesComNotaDeFechamentoOuConselhoPorAlunoEBimestre(int anoLetivo, string[] turmaId, int? bimestre, string codigoAluno);
        Task<string> ObterDescricaoPorId(long id);
        Task<IEnumerable<ComponenteCurricularDescricaoDto>> ObterDescricaoPorIds(long[] ids);
        Task<IEnumerable<ComponenteCurricularSimplesDto>> ObterComponentesSimplesPorIds(long[] ids);
        Task<string> ObterCodigoComponentePai(long componenteCurricularId);
        Task<DisciplinaDto> ObterDisciplinaPorId(long id);
        Task<ComponenteGrupoMatrizDto> ObterComponenteGrupoMatrizPorId(long id);
        Task<IEnumerable<InfoComponenteCurricular>> ObterInformacoesComponentesCurriculares();
    }
}