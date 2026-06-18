using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/armazenamento/informes")]
    public class InformesDownloadController : Controller
    {
        [HttpGet("{informativoId}/anexos/compactados")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> DownloadAnexosCompactados(long informativoId, [FromServices] IDownloadTodosAnexosInformativoUseCase useCase)
        {
            var (arquivo, contentType, nomeArquivo) = await useCase.Executar(informativoId);
            if (arquivo.EhNulo()) return NoContent();

            return File(arquivo, contentType, nomeArquivo);
        }

        [HttpGet("{codigoArquivo}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Download(Guid codigoArquivo, [FromServices] IDownloadArquivoInformativoUseCase useCase)
        {
            var (arquivo, contentType, nomeArquivo) = await useCase.Executar(codigoArquivo);
            if (arquivo.EhNulo()) return NoContent();

            return File(arquivo, contentType, nomeArquivo);
        }
    }
}
