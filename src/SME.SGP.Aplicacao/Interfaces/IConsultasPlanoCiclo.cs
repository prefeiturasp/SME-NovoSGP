using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasPlanoCiclo
    {
        PlanoCicloCompletoDto ObterPorId(long id);
    }
}