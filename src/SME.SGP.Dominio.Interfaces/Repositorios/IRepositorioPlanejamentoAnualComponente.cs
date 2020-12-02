using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanejamentoAnualComponente : IRepositorioBase<PlanejamentoAnualComponente>
    {
        Task<PlanejamentoAnualComponente> ObterPorPlanejamentoAnualPeriodoEscolarId(long componenteCurricularId, long id);
        Task RemoverLogicamenteAsync(long id);
    }
}
