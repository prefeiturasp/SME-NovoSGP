using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/compensacoes/ausencia")]
    [Authorize("Bearer")]
    public class CompensacaoAusenciaController : ControllerBase
    {
        [HttpGet()]
        [ProducesResponseType(typeof(CompensacaoAusenciaListagemDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.ADAP_C, Policy = "Bearer")]
        public async Task<IActionResult> listar([FromQuery] FiltroCompensacoesAusenciaDto filtros, [FromServices] IConsultasCompensacaoAusencia consultas)
        {
            return Ok(await consultas.Listar(filtros.TurmaId, filtros.DisciplinaId, filtros.Bimestre, filtros.AtividadeNome, filtros.AlunoNome));
        }

        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.ADAP_C, Policy = "Bearer")]
        public async Task<IActionResult> Inserir([FromBody] CompensacaoAusenciaDto compensacao, [FromServices] IComandosCompensacaoAusencia comandos)
        {
            await comandos.Inserir(compensacao);
            return Ok();
        }
    }
}
