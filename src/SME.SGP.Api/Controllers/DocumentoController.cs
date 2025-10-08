using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/armazenamento/documentos")]
    [Authorize("Bearer")]
    public class DocumentoController : ControllerBase
    {
        [HttpGet("{documentoId}/tipo-documento/{tipoDocumentoId}/classificacao/{classificacaoId}/usuario/{usuarioId}/ue/{ueId}/AnoLetivo/{AnoLetivo}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [Permissao(Permissao.DPU_C, Policy = "Bearer")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ValidacaoUsuario(long documentoId, long tipoDocumentoId, long classificacaoId, long usuarioId, long ueId,int anoLetivo, [FromServices] IVerificarUsuarioDocumentoUseCase useCase)
        {
            return Ok(await useCase.Executar(new VerificarUsuarioDocumentoDto(tipoDocumentoId, classificacaoId, usuarioId, ueId, anoLetivo,documentoId)));
        }

        [HttpGet("tipos")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [Permissao(Permissao.DPU_C, Policy = "Bearer")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ListarTiposDeDocumentos([FromServices] IListarTiposDeDocumentosUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }

        [HttpPost("")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [Permissao(Permissao.DPU_I, Policy = "Bearer")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> SalvarDocumento(SalvarDocumentoDto dto, [FromServices] ISalvarDocumentoUseCase useCase)
        {
            return Ok(await useCase.Executar(dto));
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<DocumentoDto>), 200)]
        [ProducesResponseType(401)]
        [Permissao(Permissao.DPU_C, Policy = "Bearer")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ListarDocumentos([FromQuery] FiltroListagemDocumentosDto filtro, [FromServices] IListarDocumentosUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpPost("upload")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [Permissao(Permissao.DPU_I, Permissao.DPU_A, Policy = "Bearer")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> UploadDocumentos([FromForm] IFormFile file, [FromServices] IUploadDocumentoUseCase useCase)
        {
            if (file.Length > 0)
                return Ok(await useCase.Executar(file));

            return BadRequest();
        }

        [HttpDelete("{documentoId}/arquivo/{codigoArquivo}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [Permissao(Permissao.DPU_E, Policy = "Bearer")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ExcluirArquivo(long documentoId, Guid codigoArquivo, [FromServices] IExcluirDocumentoArquivoUseCase useCase)
        {
            return Ok(await useCase.Executar((documentoId, codigoArquivo)));
        }

        [HttpDelete("{documentoId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [Permissao(Permissao.DPU_E, Policy = "Bearer")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ExcluirDocumento(long documentoId, [FromServices] IExcluirDocumentoUseCase useCase)
        {
            return Ok(await useCase.Executar(documentoId));
        }

        [HttpPut("{documentoId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [Permissao(Permissao.DPU_A, Policy = "Bearer")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> AtualizarDocumento(long documentoId, [FromBody] AlterarDocumentoDto dto,[FromServices] IAlterarDocumentoUseCase useCase)
        {
            return Ok(await useCase.Executar(new AlterarDocumentoDto { DocumentoId = documentoId, ArquivosCodigos = dto.ArquivosCodigos }));
        }

        [HttpGet("{documentoId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [Permissao(Permissao.DPU_C, Policy = "Bearer")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterDocumento(long documentoId, [FromServices] IObterDocumentoUseCase useCase)
        {
            return Ok(await useCase.Executar(documentoId));
        }
    }
}