using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanejamentoAnualPeriodoEscolar : IRepositorioBase<PlanejamentoAnualPeriodoEscolar>
    {
        Task<PlanejamentoAnualPeriodoEscolar> ObterPorPlanejamentoAnualId(long id, long periodoEscolarId);
    }
}
