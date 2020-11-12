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
    [Route("api/v1/armazenamento/documentos")]
    [Authorize("Bearer")]
    public class DocumentoController : ControllerBase
    {
        [HttpGet("tipo-documento/{tipoDocumentoId}/classificacao/{classificacaoId}/usuario/{usuarioId}/ue/{ueId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ValidacaoUsuarioDocumento(long tipoDocumentoId, long classificacaoId, long usuarioId, long ueId, [FromServices] IVerificarUsuarioDocumentoUseCase useCase)
        {
            return Ok(await useCase.Executar(new VerificarUsuarioDocumentoDto(tipoDocumentoId, classificacaoId, usuarioId, ueId)));
        }

        [HttpGet("tipos")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ListarTiposDeDocumentos([FromServices] IListarTiposDeDocumentosUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }

        [HttpPost("")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> SalvarDocumento(SalvarDocumentoDto dto, [FromServices] ISalvarDocumentoUseCase useCase)
        {
            return Ok(await useCase.Executar(dto));
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<DocumentoDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ListarDocumentos([FromQuery] long ueId, [FromQuery] long tipoDocumentoId, [FromQuery] long classificacaoId, [FromServices] IListarDocumentosUseCase useCase)
        {
            return Ok(await useCase.Executar(ueId, tipoDocumentoId, classificacaoId));
        }

        [HttpPost("upload")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> UploadDocumentos([FromForm] IFormFile file, [FromServices] IUploadDocumentoUseCase useCase)
        {
            if (file.Length > 0)
                return Ok(await useCase.Executar(file, Dominio.TipoConteudoArquivo.PDF));

            return BadRequest();
        }

        [HttpDelete("{documentoId}/arquivo/{codigoArquivo}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ExcluirArquivo(long documentoId, Guid codigoArquivo, [FromServices] IExcluirDocumentoArquivoUseCase useCase)
        {
            return Ok(await useCase.Executar((documentoId, codigoArquivo)));
        }

        [HttpDelete("{documentoId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ExcluirDocumento(long documentoId, [FromServices] IExcluirDocumentoUseCase useCase)
        {
            return Ok(await useCase.Executar(documentoId));
        }

        [HttpPut("{documentoId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> AtualizarDocumento(long documentoId, [FromBody] Guid codigoArquivo, [FromServices] IAlterarDocumentoUseCase useCase)
        {
            return Ok(await useCase.Executar(new AlterarDocumentoDto() { DocumentoId = documentoId, CodigoArquivo = codigoArquivo }));
        }

        [HttpGet("{documentoId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterDocumento(long documentoId, [FromServices] IObterDocumentoUseCase useCase)
        {
            return Ok(await useCase.Executar(documentoId));
        }
    }
}