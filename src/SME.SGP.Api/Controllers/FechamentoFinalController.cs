using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/fechamentos/finais")]
    [Authorize("Bearer")]
    public class FechamentoFinalController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(FechamentoFinalConsultaRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> Obter([FromQuery]FechamentoFinalConsultaFiltroDto filtroFechamentoFinalConsultDto, [FromServices] IConsultasFechamentoFinal consultasFechamentoFinal)
        {
            return Ok(await consultasFechamentoFinal.ObterFechamentos(filtroFechamentoFinalConsultDto));
        }

        [HttpPost]
        [ProducesResponseType(typeof(string[]), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_I, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromBody]FechamentoFinalSalvarDto fechamentoFinalSalvarDto, [FromServices]IComandosFechamentoFinal comandosFechamentoFinal)
        {
            var mensagens = await comandosFechamentoFinal.SalvarAsync(fechamentoFinalSalvarDto);
            if (mensagens != null && mensagens.Any())
                return StatusCode(601, new RetornoBaseDto() { Mensagens = mensagens.ToList() });

            return Ok();
        }
    }
}