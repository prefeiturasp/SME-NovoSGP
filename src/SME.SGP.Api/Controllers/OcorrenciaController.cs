using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/ocorrencias")]
    [Authorize("Bearer")]
    public class OcorrenciaController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OcorrenciaListagemDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        // O permissionamento será adicionado em uma task separada
        public async Task<IActionResult> Get([FromServices] IListarOcorrenciasUseCase useCase, [FromQuery] FiltroOcorrenciaListagemDto dto)
        {
            return Ok(await useCase.Executar(dto));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OcorrenciaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        // O permissionamento será adicionado em uma task separada
        public async Task<IActionResult> Get([FromServices] IObterOcorrenciaUseCase useCase, [FromQuery] long id)
        {
            var result = await useCase.Executar(id);
            if (result == null)
                return NoContent();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> Inserir([FromServices] IInserirOcorrenciaUseCase useCase, [FromBody] InserirOcorrenciaDto dto)
        {
            return Ok(await useCase.Executar(dto));
        }

        [HttpPut]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> Alterar([FromServices] IAlterarOcorrenciaUseCase useCase, [FromBody] AlterarOcorrenciaDto dto)
        {
            return Ok(await useCase.Executar(dto));
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        // O permissionamento será adicionado em uma task separada
        public async Task<IActionResult> Excluir([FromBody] IEnumerable<long> ids, [FromServices] IExcluirOcorrenciaUseCase excluirOcorrenciaUseCase)
        {
            await excluirOcorrenciaUseCase.Executar(ids);
            return await Task.FromResult(Ok());
        }
    }
}