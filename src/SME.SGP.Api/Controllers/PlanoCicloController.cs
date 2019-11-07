using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/planos/ciclo")]
    [ValidaDto]
    public class PlanoCicloController : ControllerBase
    {
        private readonly IComandosPlanoCiclo comandosPlanoCiclo;
        private readonly IConsultasPlanoCiclo consultasPlanoCiclo;

        public PlanoCicloController(IComandosPlanoCiclo comandosPlanoCiclo,
                                    IConsultasPlanoCiclo consultasPlanoCiclo)
        {
            this.comandosPlanoCiclo = comandosPlanoCiclo ?? throw new System.ArgumentNullException(nameof(comandosPlanoCiclo));
            this.consultasPlanoCiclo = consultasPlanoCiclo ?? throw new System.ArgumentNullException(nameof(consultasPlanoCiclo));
        }

        [HttpGet]
        [Route("{ano}/{cicloId}/{escolaId}")]
        [ProducesResponseType(typeof(PlanoCicloCompletoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PDC_C, Policy = "Bearer")]
        public IActionResult Get(int ano, long cicloId, string escolaId)
        {
            return Ok(consultasPlanoCiclo.ObterPorAnoCicloEEscola(ano, cicloId, escolaId));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PDC_I, Permissao.PDC_A, Policy = "Bearer")]
        public IActionResult Post(PlanoCicloDto planoCicloDto)
        {
            comandosPlanoCiclo.Salvar(planoCicloDto);
            return Ok();
        }
    }
}