using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/planos/anual/territorio-saber")]
    [ValidaDto]
    public class PlanoAnualTerritorioSaberController : Controller
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PlanoAnualTerritorioSaberCompletoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PT_C, Policy = "Bearer")]
        public async Task<IActionResult> Get(string turmaId, string ueId, int anoLetivo, long territorioExperienciaId, [FromServices]IConsultasPlanoAnualTerritorioSaber consultasPlanoAnualTerritorioSaber)
        {
            return Ok(await consultasPlanoAnualTerritorioSaber.ObterPorUETurmaAnoETerritorioExperiencia(ueId, turmaId, anoLetivo, territorioExperienciaId));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PT_I, Permissao.PT_A, Policy = "Bearer")]
        public async Task<IActionResult> Post(PlanoAnualTerritorioSaberDto planoAnualTerritorioSaberDto, [FromServices]IComandosPlanoAnualTerritorioSaber comandosPlanoAnualTerritorioSaber)
        {
            var resultado = await comandosPlanoAnualTerritorioSaber.Salvar(planoAnualTerritorioSaberDto);

            if (planoAnualTerritorioSaberDto.Id == default(int))
            {
                return Ok();
            }

            return Ok(resultado);
        }
    }
}