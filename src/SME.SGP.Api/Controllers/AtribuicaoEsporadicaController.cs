using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/atribuicao/esporadica")]
    [ValidaDto]
    public class AtribuicaoEsporadicaController : ControllerBase
    {
        [HttpGet("listar")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<AtribuicaoEsporadicaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Listar([FromQuery]FiltroAtribuicaoEsporadicaDto filtro, [FromServices]IConsultasAtribuicaoEsporadica consultasAtribuicaoEsporadica)
        {      
            return Ok(await consultasAtribuicaoEsporadica.Listar(filtro));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AE_A, Permissao.AE_I, Policy = "Bearer")]
        public IActionResult Post([FromBody]AtribuicaoEsporadicaCompletaDto atribuicaoEsporadicaCompletaDto, [FromServices]IComandosAtribuicaoEsporadica comandosAtribuicaoEsporadica)
        {
            comandosAtribuicaoEsporadica.Salvar(atribuicaoEsporadicaCompletaDto);

            return Ok();
        }
    }
}