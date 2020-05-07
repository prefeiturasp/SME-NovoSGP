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
        [Permissao(Permissao.PA_C, Policy = "Bearer")]
        public IActionResult Get(string turmaId, string ueId, int anoLetivo, long territorioExperienciaId, [FromServices]IConsultasPlanoAnualTerritorioSaber consultasPlanoAnualTerritorioSaber)
        {
            return Ok(consultasPlanoAnualTerritorioSaber.ObterPorUETurmaAnoETerritorioExperiencia(ueId, turmaId, anoLetivo, territorioExperienciaId));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_I, Permissao.PA_A, Policy = "Bearer")]
        public IActionResult Post(PlanoAnualTerritorioSaberDto planoAnualTerritorioSaberDto, [FromServices]IComandosPlanoAnualTerritorioSaber comandosPlanoAnualTerritorioSaber)
        {
            comandosPlanoAnualTerritorioSaber.Salvar(planoAnualTerritorioSaberDto);
            return Ok();
        }
    }
}