using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioComunicado : IRepositorioBase<Comunicado>
    {
        Task<PaginacaoResultadoDto<Comunicado>> ListarPaginado(FiltroComunicadoDto filtro, Paginacao paginacao);

        Task<ComunicadosTotaisSmeResultado> ObterComunicadosTotaisSme(int anoLetivo);
    }
}