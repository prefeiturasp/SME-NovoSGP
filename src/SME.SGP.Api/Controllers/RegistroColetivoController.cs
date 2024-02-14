using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/registro-coletivo")]
    [Authorize("Bearer")]
    public class RegistroColetivoController : ControllerBase
    {
        [HttpGet("tipo-reuniao")]
        [ProducesResponseType(typeof(TipoReuniaoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RC_NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterTipoDeReuniaoNAAPA([FromServices] IObterTiposDeReuniaoUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }

        [HttpPost("salvar")]
        [ProducesResponseType(typeof(ResultadoRegistroColetivoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RC_NAAPA_I, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromBody] RegistroColetivoDto registroColetivo, [FromServices] ISalvarRegistroColetivoUseCase useCase)
        {
            return Ok(await useCase.Executar(registroColetivo));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RC_NAAPA_E, Policy = "Bearer")]
        public async Task<IActionResult> Excluir(long id, [FromServices] IExcluirRegistroColetivoUseCase useCase)
        {
            return Ok(await useCase.Executar(id));
        }

        [HttpPost("upload")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RC_NAAPA_I, Policy = "Bearer")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromServices] IUploadDeArquivoUseCase useCase)
        {
            if (file.Length > 0)
                return Ok(await useCase.Executar(file, TipoArquivo.RegistroColetivo));

            return BadRequest();
        }
    }
}
