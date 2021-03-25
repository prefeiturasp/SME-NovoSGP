using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/supervisores")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class SupervisorController : ControllerBase
    {
        private readonly IConsultasSupervisor consultasSupervisor;

        public SupervisorController(IConsultasSupervisor consultasSupervisor)
        {
            this.consultasSupervisor = consultasSupervisor ?? throw new System.ArgumentNullException(nameof(consultasSupervisor));
        }

        [HttpPost("atribuir-ue")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ASP_I, Permissao.ASP_A, Policy = "Bearer")]
        public IActionResult AtribuirUE(AtribuicaoSupervisorUEDto atribuicaoSupervisorUEDto, [FromServices] IComandosSupervisor comandosSupervisor)
        {
            comandosSupervisor.AtribuirUE(atribuicaoSupervisorUEDto);
            return Ok();
        }

        [HttpGet("ues/{ueId}/vinculo")]
        [ProducesResponseType(typeof(SupervisorEscolasDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ASP_C, Policy = "Bearer")]
        public IActionResult ObterPorUe(string ueId)
        {
            return Ok(consultasSupervisor.ObterPorUe(ueId));
        }

        [HttpGet("dre/{dreId}")]
        [ProducesResponseType(typeof(IEnumerable<SupervisorDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ASP_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSupervidoresPorDreENome(string dreId, [FromQuery]BuscaSupervisorPorNomeDto supervisorNome)
        {
            return Ok(await consultasSupervisor.ObterPorDreENomeSupervisorAsync(supervisorNome.Nome, dreId));
        }

        [HttpGet("dre/{dreId}/vinculo-escolas")]
        [ProducesResponseType(typeof(IEnumerable<SupervisorEscolasDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ASP_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSupervisoresEEscolasPorDre(string dreId)
        {
            var retorno = await consultasSupervisor.ObterPorDre(dreId);
            if (retorno.Any())
                return Ok(retorno);
            else return StatusCode(204);
        }

        [HttpGet("{supervisoresId}/dre/{dreId}")]
        [ProducesResponseType(typeof(IEnumerable<SupervisorEscolasDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ASP_C, Policy = "Bearer")]
        public IActionResult ObterSupervisoresEEscolasPorSupervisoresEDre(string supervisoresId, string dreId)
        {
            var listaretorno = consultasSupervisor.ObterPorDreESupervisores(supervisoresId.Split(","), dreId);

            if (listaretorno == null)
                return new StatusCodeResult(204);
            else return Ok(listaretorno);
        }
    }
}