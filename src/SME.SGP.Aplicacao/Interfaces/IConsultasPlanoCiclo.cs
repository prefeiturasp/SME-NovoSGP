using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasPlanoCiclo
    {
        PlanoCicloCompletoDto ObterPorAnoECiclo(int ano, long cicloId, long escolaId);
    }
}