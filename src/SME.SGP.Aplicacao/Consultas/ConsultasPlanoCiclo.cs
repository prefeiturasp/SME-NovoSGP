using SME.SGP.Dominio;
using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ConsultasPlanoCiclo : IConsultasPlanoCiclo
    {
        private readonly IRepositorioPlanoCiclo repositorioPlanoCiclo;

        public ConsultasPlanoCiclo(IRepositorioPlanoCiclo repositorioPlanoCiclo)
        {
            this.repositorioPlanoCiclo = repositorioPlanoCiclo ?? throw new System.ArgumentNullException(nameof(repositorioPlanoCiclo));
        }

        public IEnumerable<PlanoCicloDto> Listar()
        {
            return new List<PlanoCicloDto>();
        }
    }
}
