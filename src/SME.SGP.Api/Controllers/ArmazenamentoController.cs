using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromServices] IUploadDeArquivoUseCase useCase)
        {
            if (file.Length > 0)
                return Ok(await useCase.Executar(file));

            return BadRequest();
        }

        [HttpGet("{codigoArquivo}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Download(Guid codigoArquivo, [FromServices] IDownloadDeArquivoUseCase useCase)
        {
            var (arquivo, contentType, nomeArquivo) = await useCase.Executar(codigoArquivo);
            if (arquivo.EhNulo()) return NoContent();

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
        
        [HttpDelete()]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ExcluirArquivos([FromBody]IEnumerable<Guid> codigos,[FromServices] IExcluirArquivosUseCase useCase)
        {
            return Ok(await useCase.Executar(codigos));
        }
    }
}