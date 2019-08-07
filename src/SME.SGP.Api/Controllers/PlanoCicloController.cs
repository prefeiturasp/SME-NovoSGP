using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/planos-ciclo")]
    [ValidaDto]
    public class PlanoCicloController : ControllerBase
    {
        private readonly IComandosPlanoCiclo comandosPlanoCiclo;

        public PlanoCicloController(IComandosPlanoCiclo comandosPlanoCiclo)
        {
            this.comandosPlanoCiclo = comandosPlanoCiclo ?? throw new System.ArgumentNullException(nameof(comandosPlanoCiclo));
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post(PlanoCicloDto planoCicloDto)
        {
            comandosPlanoCiclo.Salvar(planoCicloDto);
            return Ok();
        }
    }
}