using Microsoft.AspNetCore.Http;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IUploadDeArquivoItineranciaUseCase
    {
        Task<ArquivoArmazenadoItineranciaDto> Executar(IFormFile file);
    }
}