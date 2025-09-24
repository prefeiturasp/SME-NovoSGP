using Microsoft.AspNetCore.Http;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo.FluenciaLeitora
{
    public interface IImportacaoArquivoFluenciaLeitoraUseCase
    {
        Task<ImportacaoLogRetornoDto> Executar(IFormFile arquivo, int anoLetivo, int tipoAvaliacao);
    }
}
