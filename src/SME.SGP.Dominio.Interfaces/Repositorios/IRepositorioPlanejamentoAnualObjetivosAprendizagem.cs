using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanejamentoAnualObjetivosAprendizagem : IRepositorioBase<PlanejamentoAnualObjetivoAprendizagem>
    {
        void SalvarVarios(IEnumerable<PlanejamentoAnualObjetivoAprendizagem> objetivos, long planejamentoAnualComponenteId);
        Task RemoverTodosPorPlanejamentoAnualPeriodoEscolarId(long id);
        Task RemoverTodosPorPlanejamentoAnualPeriodoEscolarIdEComponenteCurricularId(long id, long componenteCurricularId);
        Task<IEnumerable<PlanejamentoAnualObjetivoAprendizagem>> ObterPorPlanejamentoAnualComponenteId(long componenteId, bool consideraExcluido = false);
        Task<IEnumerable<PlanejamentoAnualObjetivoAprendizagem>> ObterPorPlanejamentoAnualComponenteId(long[] componentesId);
    }
}
