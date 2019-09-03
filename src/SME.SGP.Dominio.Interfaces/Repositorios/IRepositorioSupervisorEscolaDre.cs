using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioSupervisorEscolaDre : IRepositorioBase<SupervisorEscolaDre>
    {
        IEnumerable<SupervisorEscolasDreDto> ObtemSupervisoresEscola(string dreId, string supervisorId);
    }
}