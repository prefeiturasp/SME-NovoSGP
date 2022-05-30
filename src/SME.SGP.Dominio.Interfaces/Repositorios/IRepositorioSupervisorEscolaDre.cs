using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioSupervisorEscolaDre : IRepositorioBase<SupervisorEscolaDre>
    {
        Task<IEnumerable<SupervisorEscolasDreDto>> ObtemPorDreESupervisor(string dreId, string supervisorId, bool excluidos = false);
        IEnumerable<SupervisorEscolasDreDto> ObtemPorDreESupervisores(string dreId, string[] supervisoresId);
        Task<IEnumerable<SupervisorEscolasDreDto>> ObtemPorUe(FiltroObterSupervisorEscolasDto filtro);
        IEnumerable<SupervisorEscolasDreDto> ObtemSupervisoresPorUe(string ueId);
        Task<IEnumerable<SupervisorEscolasDreDto>> ObtemSupervisoresPorUeAsync(string codigoUe);
        Task<IEnumerable<SupervisorEscolasDreDto>> ObtemSupervisoresPorDreAsync(string codigoDre, TipoResponsavelAtribuicao tipoResponsavelAtribuicao);
        Task<IEnumerable<DadosAbrangenciaSupervisorDto>> ObterDadosAbrangenciaSupervisor(string rfSupervisor, bool consideraHistorico, int anoLetivo);
    }
}