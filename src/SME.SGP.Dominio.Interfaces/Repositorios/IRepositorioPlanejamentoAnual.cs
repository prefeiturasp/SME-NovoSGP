using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanejamentoAnual : IRepositorioBase<PlanejamentoAnual>
    {
        Task SalvarPlanejamentoPeriodoEscolarAsync(PlanejamentoAnualPeriodoEscolar planejamentoPeriodoEscolar);
        Task SalvarPlanejamentoAnualComponenteAsync(PlanejamentoAnualComponente planejamentoAnualComponente);
    }
}
