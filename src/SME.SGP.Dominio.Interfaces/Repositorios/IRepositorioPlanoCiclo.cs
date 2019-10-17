using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanoCiclo : IRepositorioBase<PlanoCiclo>
    {
        PlanoCicloCompletoDto ObterPlanoCicloComMatrizesEObjetivos(int ano, long cicloId, string escolaId);

        bool ObterPlanoCicloPorAnoCicloEEscola(int ano, long cicloId, string escolaId);
    }
}