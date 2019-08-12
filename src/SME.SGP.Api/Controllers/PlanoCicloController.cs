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
        private readonly IConsultasPlanoCiclo consultasPlanoCiclo;

        public PlanoCicloController(IComandosPlanoCiclo comandosPlanoCiclo,
                                    IConsultasPlanoCiclo consultasPlanoCiclo)
        {
            this.comandosPlanoCiclo = comandosPlanoCiclo ?? throw new System.ArgumentNullException(nameof(comandosPlanoCiclo));
            this.consultasPlanoCiclo = consultasPlanoCiclo ?? throw new System.ArgumentNullException(nameof(consultasPlanoCiclo));
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(consultasPlanoCiclo.ObterPorId(1));
        }

        [HttpPost]
        public IActionResult Post(PlanoCicloDto planoCicloDto)
        {
            comandosPlanoCiclo.Salvar(planoCicloDto);
            return Ok();
        }
    }
}