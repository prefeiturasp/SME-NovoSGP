using SME.SGP.Aplicacao.Queries.ImportarArquivo;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo
{
    public interface IImportacaoLogUseCase : IUseCase<FiltroPesquisaImportacaoDto, PaginacaoResultadoDto<ImportacaoLogQueryRetornoDto>>
    {
    }
}
