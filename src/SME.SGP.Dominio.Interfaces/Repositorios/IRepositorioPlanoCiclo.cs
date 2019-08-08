using SME.SGP.Dto;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanoCiclo : IRepositorioBase<PlanoCiclo>
    {
        PlanoCicloDto ObterPlanoCicloComMatrizesEObjetivos(long id);
    }
}