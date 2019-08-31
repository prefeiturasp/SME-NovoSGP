using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioSupervisorEscolaDre
    {
        IEnumerable<SupervisorEscolasDreDto> ObtemSupervisoresEscola(string dreId, string supervisorId);

        IEnumerable<SupervisorEscolasDreDto> ObtemSupervisoresPorDre(string dreId);
    }
}