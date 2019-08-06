using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Dto;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/planos-ciclo")]
    [ValidaDto]
    public class PlanoCicloController : ControllerBase
    {
        private readonly IManterPlanoCiclo manterPlanoCiclo;

        public PlanoCicloController(IManterPlanoCiclo manterPlanoCiclo)
        {
            this.manterPlanoCiclo = manterPlanoCiclo ?? throw new System.ArgumentNullException(nameof(manterPlanoCiclo));
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post(PlanoCicloDto planoCicloDto)
        {
            manterPlanoCiclo.Salvar(planoCicloDto);
            return Ok();
        }
    }
}