using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanejamentoAnualComponente : IRepositorioBase<PlanejamentoAnualComponente>
    {
        Task RemoverPorPlanejamentoAnual(long id);
        Task<PlanejamentoAnualComponente> ObterPorPlanejamentoAnualPeriodoEscolarId(long componenteCurricularId, long id);
    }
}
