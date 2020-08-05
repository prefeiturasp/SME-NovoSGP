using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.CasosDeUso.DiarioBordo;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/diario-bordo")]
    [Authorize("Bearer")]
    public class DiarioBordoController : ControllerBase
    {

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DiarioBordoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> Obter(long id, [FromServices] IMediator mediator)
        {
            return Ok(await ObterDiarioBordoPorIdUseCase.Executar(mediator, id));
        }
    }
}
