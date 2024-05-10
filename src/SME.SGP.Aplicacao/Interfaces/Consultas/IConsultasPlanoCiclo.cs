using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasPlanoCiclo
    {
        PlanoCicloCompletoDto ObterPorAnoCicloEEscola(int ano, long cicloId, string escolaId);
    }
}