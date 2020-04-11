using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioComunicado : IRepositorioBase<Comunicado>
    {
        Task<PaginacaoResultadoDto<Comunicado>> ListarPaginado(FiltroComunicadoDto filtro, Paginacao paginacao);

        Task<IEnumerable<ComunicadoResultadoDto>> ObterPorIdAsync(long id);
    }
}