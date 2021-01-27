using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/armazenamento")]
    [Authorize("Bearer")]
    public class ArmazenamentoController : ControllerBase
    {
        [HttpPost("upload")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[RequestSizeLimit(200 * 1024 * 1024)]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromServices] IUploadDeArquivoUseCase useCase)
        {
            try
            {
                if (file.Length > 0)
                    return Ok(await useCase.Executar(file));

                return BadRequest();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("{codigoArquivo}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Download(Guid codigoArquivo, [FromServices] IDownloadDeArquivoUseCase useCase)
        {
            var (arquivo, contentType, nomeArquivo) = await useCase.Executar(codigoArquivo);
            return File(arquivo, contentType, nomeArquivo);
        }

        [HttpDelete("{codigoArquivo}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Delete(Guid codigoArquivo, [FromServices] IExcluirArquivoUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoArquivo));
        }
    }
}