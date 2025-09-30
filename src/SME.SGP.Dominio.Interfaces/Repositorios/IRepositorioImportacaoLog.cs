using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioImportacaoLog : IRepositorioBase<ImportacaoLog>
    {
        Task<PaginacaoResultadoDto<ImportacaoLog>> ObterImportacaoLogPaginada(Paginacao paginacao, FiltroPesquisaImportacaoDto filtro);
    }
}
