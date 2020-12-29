using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Ocorrencias.Tipos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/ocorrencias/tipos")]
    //[Authorize("Bearer")]
    public class OcorrenciaTipoController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OcorrenciaTipoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        // O permissionamento será adicionado em uma task separada
        public async Task<IActionResult> Get()
        {
            var resultado = new List<OcorrenciaTipoDto>
            {
                new OcorrenciaTipoDto { Descricao = "Briga", Id = 1},
                new OcorrenciaTipoDto { Descricao = "Vandalismo", Id = 2},
                new OcorrenciaTipoDto { Descricao = "Acidente", Id = 3}
            };

            return await Task.FromResult(Ok(resultado));
        }
    }
}