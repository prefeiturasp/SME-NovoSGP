using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanejamentoAnualPeriodoEscolar : IRepositorioBase<PlanejamentoAnualPeriodoEscolar>
    {
        Task<PlanejamentoAnualPeriodoEscolar> ObterPorPlanejamentoAnualIdEPeriodoId(long id, long periodoEscolarId);
        Task<IEnumerable<PlanejamentoAnualPeriodoEscolarResumoDto>> ObterPorPlanejamentoAnualId(long planejamentoAnualId);
        Task<PlanejamentoAnualPeriodoEscolar> ObterPlanejamentoAnualPeriodoEscolarPorTurmaEComponenteCurricular(long turmaId, long componenteCurricularId, long periodoEscolarId);
    }
}
