using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IUploadDeArquivoItineranciaUseCase
    {
       Task<ArquivoArmazenadoItineranciaDto>Executar(IFormFile file);
    }
}