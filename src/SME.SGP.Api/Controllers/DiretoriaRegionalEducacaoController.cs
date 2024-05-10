


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
    [Route("api/v1/dres")]
    [Authorize("Bearer")]
    public class DiretoriaRegionalEducacaoController : ControllerBase
    {
        private readonly IConsultaDres consultaDres;

        public DiretoriaRegionalEducacaoController(IConsultaDres consultaDres)
        {
            this.consultaDres = consultaDres ?? throw new System.ArgumentNullException(nameof(consultaDres));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CicloDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ARP_I, Permissao.ARP_A, Permissao.ARP_C, Policy = "Bearer")]
        public async Task<IActionResult> Get()
        {
            return Ok(await consultaDres.ObterTodos());
        }

        [HttpGet("{dreId}/ues/sem-atribuicao/{tipoResponsavel}")]
        [ProducesResponseType(typeof(IEnumerable<UnidadeEscolarDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ARP_I, Permissao.ARP_A, Permissao.ARP_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterEscolasSemAtribuicao(string dreId, int tipoResponsavel)
        {
            var retorno = await consultaDres.ObterEscolasSemAtribuicao(dreId, tipoResponsavel);
            if (retorno.Any())
                return Ok(retorno);
            else return StatusCode(204);
        }

        [HttpGet("{dreId}/ues")]
        [ProducesResponseType(typeof(IEnumerable<UnidadeEscolarDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ARP_I, Permissao.ARP_A, Permissao.ARP_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterUesPorDre(string dreId)
        {
            var retorno = await consultaDres.ObterEscolasPorDre(dreId);
            if (retorno.Any())
                return Ok(retorno);
            else return StatusCode(204);
        }
    }
}