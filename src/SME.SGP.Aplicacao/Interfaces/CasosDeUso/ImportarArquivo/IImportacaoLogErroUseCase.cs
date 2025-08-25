using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo
{
    public interface IImportacaoLogErroUseCase : IUseCase<FiltroPesquisaImportacaoDto, PaginacaoResultadoDto<ImportacaoLogErroQueryRetornoDto>>
    {
    }
}
