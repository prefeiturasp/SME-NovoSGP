using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/encaminhamento-aee")]
    public class EncaminhamentoAeeController : ControllerBase
    {
        //[HttpGet("questionario")]
        //[ProducesResponseType(typeof(IEnumerable<QuestaoAeeDto>), 200)]
        //[ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //public async Task<IActionResult> ObterQuestionario([FromQuery] long questionarioId, [FromQuery] long? encaminhamentoId, [FromServices] IObterQuestionarioEncaminhamentoAeeUseCase useCase)
        //{
        //    return Ok(await useCase.Executar(questionarioId, encaminhamentoId));
        //}


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
                    return Ok(await useCase.Executar(file, Dominio.TipoArquivo.EncaminhamentoAEE));

                return BadRequest();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
