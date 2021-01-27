using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioSupervisorEscolaDre : IRepositorioBase<SupervisorEscolaDre>
    {
        IEnumerable<SupervisorEscolasDreDto> ObtemPorDreESupervisor(string dreId, string supervisorId, bool excluidos = false);

        IEnumerable<SupervisorEscolasDreDto> ObtemPorDreESupervisores(string dreId, string[] supervisoresId);

        SupervisorEscolasDreDto ObtemPorUe(string ueId);

        IEnumerable<SupervisorEscolasDreDto> ObtemSupervisoresPorUe(string ueId);

        Task<IEnumerable<SupervisorEscolasDreDto>> ObtemSupervisoresPorUeAsync(string codigoUe);
        Task<IEnumerable<SupervisorEscolasDreDto>> ObtemSupervisoresPorDreAsync(string codigoDre);
    }
}