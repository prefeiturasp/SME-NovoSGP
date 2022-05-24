using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
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
            this.consultasSupervisor = consultasSupervisor ?? throw new ArgumentNullException(nameof(consultasSupervisor));
        }

        [HttpPost("atribuir-ue")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ARP_I, Permissao.ARP_A, Policy = "Bearer")]
        public async Task<IActionResult> AtribuirUE(AtribuicaoSupervisorUEDto atribuicaoSupervisorUEDto,
            [FromServices] ISupervisorAtribuirUeUseCase useCase)
        {
            return Ok(await useCase.Executar(atribuicaoSupervisorUEDto));
        }

        [HttpGet("vinculo-lista")]
        [ProducesResponseType(typeof(SupervisorEscolasDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ARP_C, Policy = "Bearer")]
        public IActionResult ObterPorUe([FromQuery] FiltroObterSupervisorEscolasDto filtro)
        {
            return Ok(consultasSupervisor.ObterPorUe(filtro));
        }


        [HttpGet("tipo-responsavel")]
        public IActionResult ObterListTipoReponsavel()
        {
            var tipos = Enum.GetValues(typeof(TipoResponsavelAtribuicao))
                .Cast<TipoResponsavelAtribuicao>()
                .Select(d => new { codigo = (int)d, descricao = d.Name() }).OrderBy(x => x.descricao).ToList();

            return Ok(tipos);
        }

        [HttpGet("dre/{dreId}")]
        [ProducesResponseType(typeof(IEnumerable<SupervisorDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ARP_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSupervisoresPorDre(string dreId, [FromQuery]FiltroObterSupervisoresDto filtro,
            [FromServices] IObterSupervisoresPorDreUseCase useCase)
        {
            return Ok(await useCase.Executar(new ObterSupervisoresPorDreDto(dreId, filtro.TipoResponsavelAtribuicao)));
        }

        [HttpGet("dre/{dreId}/vinculo-escolas")]
        [ProducesResponseType(typeof(IEnumerable<SupervisorEscolasDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ARP_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSupervisoresEEscolasPorDre(string dreId)
        {
            var retorno = await consultasSupervisor.ObterPorDre(dreId);

            if (retorno.Any())
                return Ok(retorno);
            else 
                return StatusCode(204);
        }

        [HttpGet("{supervisoresId}/dre/{dreId}")]
        [ProducesResponseType(typeof(IEnumerable<SupervisorEscolasDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ARP_C, Policy = "Bearer")]
        public IActionResult ObterSupervisoresEEscolasPorSupervisoresEDre(string supervisoresId, string dreId)
        {
            var listaretorno = consultasSupervisor.ObterPorDreESupervisores(supervisoresId.Split(","), dreId);

            if (listaretorno == null)
                return new StatusCodeResult(204);
            else 
                return Ok(listaretorno);
        }
    }
}