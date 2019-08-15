using SME.SGP.Dto;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanoCiclo : IRepositorioBase<PlanoCiclo>
    {
        PlanoCicloCompletoDto ObterPlanoCicloComMatrizesEObjetivos(int ano, long cicloId, long escolaId);

        bool ObterPlanoCicloPorAnoCicloEEscola(int ano, long cicloId, long escolaId);
    }
}