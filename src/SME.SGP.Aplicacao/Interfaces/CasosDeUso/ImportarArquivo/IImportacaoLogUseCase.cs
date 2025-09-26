using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo
{
    public interface IImportacaoLogUseCase : IUseCase<FiltroPesquisaImportacaoDto, PaginacaoResultadoDto<ImportacaoLogQueryRetornoDto>>
    {
        Task<PaginacaoResultadoDto<ImportacaoLogQueryRetornoDto>> Executar(FiltroPesquisaImportacaoDto filtro, int numeroPagina, int numeroRegistros);
    }
}
