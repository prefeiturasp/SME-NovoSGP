using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;

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
        public IActionResult Get(int ano, long cicloId, string escolaId)
        {
            return Ok(consultasPlanoCiclo.ObterPorAnoCicloEEscola(ano, cicloId, escolaId));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Post(PlanoCicloDto planoCicloDto)
        {
            comandosPlanoCiclo.Salvar(planoCicloDto);
            return Ok();
        }
    }
}