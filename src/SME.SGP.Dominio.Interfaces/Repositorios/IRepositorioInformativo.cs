using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Informes;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioInformativo : IRepositorioBase<Informativo>
    {
        Task<Informativo> ObterInformes(long id);
        Task<PaginacaoResultadoDto<Informativo>> ObterInformesPaginado(InformeFiltroDto filtro, Paginacao paginacao);
        Task<bool> InformeFoiExcluido(long id);
    }
}
