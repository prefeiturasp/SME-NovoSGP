using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/encaminhamento-naapa")]
    // [Authorize("Bearer")]
    public class EncaminhamentoNAAPAController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<EncaminhamentoNAAPAResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        // [Permissao(Permissao.NAAPA_C, Policy = "Bearer")] 
        public async Task<IActionResult> ObterEncaminhamentosNAAPA([FromQuery] FiltroEncaminhamentoNAAPADto filtro, [FromServices] IObterEncaminhamentoNAAPAUseCase useCase)
        {
            filtro = new FiltroEncaminhamentoNAAPADto()
            {
                ExibirHistorico = true,
                AnoLetivo = 2022,
                DreId = 7,
                CodigoUe = "307288",
                TurmaId = 1549249,
                //DataAberturaQueixaInicio = new DateTime(2022, 11, 1),
                //DataAberturaQueixaFim = new DateTime(2022, 11, 18),
                Situacao = 1,
                Prioridade = 1,
            };
            
            return Ok(await useCase.Executar(filtro));
        }
    }
}
