using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanejamentoAnualPeriodoEscolar : IRepositorioBase<PlanejamentoAnualPeriodoEscolar>
    {
        Task<PlanejamentoAnualPeriodoEscolar> ObterPorPlanejamentoAnualIdEPeriodoId(long id, long periodoEscolarId, bool consideraxcluido = false);
        Task<IEnumerable<PlanejamentoAnualPeriodoEscolarResumoDto>> ObterPorPlanejamentoAnualId(long planejamentoAnualId, int[] bimestresConsiderados);
        Task<IEnumerable<PlanejamentoAnualPeriodoEscolarResumoDto>> ObterPlanejamentosAnuaisPeriodosTurmaPorPlanejamentoAnualId(long planejamentoAnualId);
        Task<PlanejamentoAnualPeriodoEscolar> ObterPlanejamentoAnualPeriodoEscolarPorTurmaEComponenteCurricular(long turmaId, long componenteCurricularId, long periodoEscolarId);

        Task<IEnumerable<PlanejamentoAnualPeriodoEscolar>> ObterCompletoPorIdAsync(long[] ids);
        Task<bool> PlanejamentoPossuiObjetivos(long planejamentoAnualPeriodoId);
        Task RemoverLogicamenteAsync(long id);
        Task RemoverLogicamentePorTurmaBimestreAsync(long idTurma, int bimestre);
    }
}
