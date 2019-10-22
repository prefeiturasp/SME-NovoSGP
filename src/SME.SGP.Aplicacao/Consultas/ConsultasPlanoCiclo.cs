using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ConsultasPlanoCiclo : IConsultasPlanoCiclo
    {
        private readonly IRepositorioPlanoCiclo repositorioPlanoCiclo;

        public ConsultasPlanoCiclo(IRepositorioPlanoCiclo repositorioPlanoCiclo)
        {
            this.repositorioPlanoCiclo = repositorioPlanoCiclo ?? throw new System.ArgumentNullException(nameof(repositorioPlanoCiclo));
        }

        public PlanoCicloCompletoDto ObterPorAnoCicloEEscola(int ano, long cicloId, string escolaId)
        {
            return repositorioPlanoCiclo.ObterPlanoCicloComMatrizesEObjetivos(ano, cicloId, escolaId);
        }
    }
}