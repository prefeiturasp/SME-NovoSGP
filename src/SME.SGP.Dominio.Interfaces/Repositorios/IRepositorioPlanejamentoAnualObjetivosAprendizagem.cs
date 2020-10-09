using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanejamentoAnualObjetivosAprendizagem : IRepositorioBase<PlanejamentoAnualObjetivoAprendizagem>
    {
        void SalvarVarios(IEnumerable<PlanejamentoAnualObjetivoAprendizagem> objetivos, long planejamentoAnualComponenteId);
        Task RemoverTodosPorPlanejamentoAnualPeriodoEscolarId(long id);

        Task<IEnumerable<PlanejamentoAnualObjetivoAprendizagem>> ObterPorPlanejamentoAnualComponenteId(long componenteId);
    }
}
