using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanejamentoAnualComponente : IRepositorioBase<PlanejamentoAnualComponente>
    {
        Task<PlanejamentoAnualComponente> ObterPorPlanejamentoAnualPeriodoEscolarId(long componenteCurricularId, long id, bool consideraExcluido = false);
        Task<IEnumerable<PlanejamentoAnualComponente>> ObterListaPorPlanejamentoAnualPeriodoEscolarId(long turmaId, long componenteCurricularId, int bimestre);
        Task RemoverLogicamenteAsync(long[] ids);
    }
}
