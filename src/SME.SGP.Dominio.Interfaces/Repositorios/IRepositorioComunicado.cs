using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioComunicado : IRepositorioBase<Comunicado>
    {
        Task<PaginacaoResultadoDto<Comunicado>> ListarPaginado(FiltroComunicadoDto filtro, Paginacao paginacao);

        Task<ComunicadosTotaisResultado> ObterComunicadosTotaisSme(int anoLetivo, string codigoDre, string codigoUe);

        Task<IEnumerable<ComunicadosTotaisPorDreResultado>> ObterComunicadosTotaisAgrupadosPorDre(int anoLetivo);

        Task<IEnumerable<ComunicadoParaFiltroDaDashboardDto>> ObterComunicadosParaFiltroDaDashboard(FiltroObterComunicadosParaFiltroDaDashboardDto filtro);
        
        Task<bool> VerificaExistenciaComunicadoParaEvento(long eventoId);

        Task<IEnumerable<ComunicadoTurmaDto>> ObterComunicadosTurma(long comunicadoId);

    }
}