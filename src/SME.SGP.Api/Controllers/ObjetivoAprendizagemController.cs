using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/objetivos-aprendizagem")]
    [ValidaDto]
    public class ObjetivoAprendizagemController : ControllerBase
    {
        private readonly IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem;

        public ObjetivoAprendizagemController(IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem)
        {
            this.consultasObjetivoAprendizagem = consultasObjetivoAprendizagem ?? throw new System.ArgumentNullException(nameof(consultasObjetivoAprendizagem));
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<ObjetivoAprendizagemDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Filtrar([FromBody]FiltroObjetivosAprendizagemDto filtroObjetivosAprendizagemDto)
        {
            return Ok(await consultasObjetivoAprendizagem.Filtrar(filtroObjetivosAprendizagemDto));
        }
    }
}