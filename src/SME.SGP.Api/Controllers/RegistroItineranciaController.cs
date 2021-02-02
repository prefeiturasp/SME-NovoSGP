using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api
{
    [ApiController]
    [Route("api/v1/registros-itinerancias")]
    //[Authorize("Bearer")]
    public class RegistroItineranciaController : ControllerBase
    {

        [HttpGet("/obetivos")]
        [ProducesResponseType(typeof(RegistroIndividualDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.REI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterObjetivos()
        {
            var objetivos = new List<RegistroItineranciaObjetivoDto>()
            {
                new RegistroItineranciaObjetivoDto(1, "Mapeamento dos estudantes público da Educação Especial", false, false),
                new RegistroItineranciaObjetivoDto(2, "Reorganização e/ou remanejamento de apoios e serviços", false, false),
                new RegistroItineranciaObjetivoDto(3, "Atendimento de solicitação da U.E", true, false),
                new RegistroItineranciaObjetivoDto(4, "Acompanhamento professor de sala regular", false, true),
                new RegistroItineranciaObjetivoDto(5, "Acompanhamento professor de SRM", false, true),
                new RegistroItineranciaObjetivoDto(6, "Ação Formativa em JEIF", false, true),
                new RegistroItineranciaObjetivoDto(7, "Reunião", false, true),
                new RegistroItineranciaObjetivoDto(8, "Outros", true, false),
            };
            return Ok(objetivos);
        }
    }
}