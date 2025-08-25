using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioImportacaoLogErro : IRepositorioBase<ImportacaoLogErro>
    {
        Task<PaginacaoResultadoDto<ImportacaoLogErro>> ObterImportacaoLogErroPaginada(Paginacao paginacao, FiltroPesquisaImportacaoDto filtro);
    }
}
