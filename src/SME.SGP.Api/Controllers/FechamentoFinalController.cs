using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
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
        public IActionResult Obter([FromQuery]FechamentoFinalConsultaFiltroDto filtroFechamentoFinalConsultDto)
        {
         

            return Ok(null);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string[]), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        //[Permissao(Permissao.FB_I, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromBody]FechamentoFinalSalvarDto fechamentoFinalSalvarDto, [FromServices]IComandosFechamentoFinal comandosFechamentoFinal)
        {
            return Ok(await comandosFechamentoFinal.SalvarAsync(fechamentoFinalSalvarDto));
        }
    }
}