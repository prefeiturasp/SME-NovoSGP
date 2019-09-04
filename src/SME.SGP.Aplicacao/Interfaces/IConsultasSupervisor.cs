using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasSupervisor
    {
        IEnumerable<SupervisorEscolasDto> ObterPorDre(string dreId);

        IEnumerable<SupervisorDto> ObterPorDreENomeSupervisor(string supervisorNome, string dreId);

        IEnumerable<SupervisorEscolasDto> ObterPorDreESupervisor(string supervisorId, string dreId);
    }
}