using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasPlanoCiclo
    {
        IEnumerable<PlanoCicloDto> Listar();

        PlanoCicloDto ObterPorId(long id);
    }
}