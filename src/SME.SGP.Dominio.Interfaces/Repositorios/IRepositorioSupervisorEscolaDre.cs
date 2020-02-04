using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioSupervisorEscolaDre : IRepositorioBase<SupervisorEscolaDre>
    {
        IEnumerable<SupervisorEscolasDreDto> ObtemPorDreESupervisor(string dreId, string supervisorId, bool excluidos = false);

        IEnumerable<SupervisorEscolasDreDto> ObtemPorDreESupervisores(string dreId, string[] supervisoresId);

        SupervisorEscolasDreDto ObtemPorUe(string ueId);

        IEnumerable<SupervisorEscolasDreDto> ObtemSupervisoresPorUe(string ueId);
    }
}