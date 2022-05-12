using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/atribuicao/poa")]
    [ValidaDto]
    public class RegistroPoaController : ControllerBase
    {
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.RPOA_E, Policy = "Bearer")]
        public IActionResult Delete(long id, [FromServices]IComandosRegistroPoa comandosRegistroPoa)
        {
            comandosRegistroPoa.Excluir(id);

            return Ok();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RegistroPoaCompletoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.RPOA_C, Policy = "Bearer")]
        public IActionResult Get(long id, [FromServices]IConsultasRegistroPoa consultasRegistroPoa)
        {
            var retorno = consultasRegistroPoa.ObterPorId(id);

            if (retorno != null)
                return Ok(retorno);

            return NoContent();
        }

        [HttpGet("listar")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<RegistroPoaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.RPOA_C, Policy = "Bearer")]
        public async Task<IActionResult> Listar([FromQuery]RegistroPoaFiltroDto registroPoaFiltroDto, [FromServices]IConsultasRegistroPoa consultasRegistroPoa)
        {
            return Ok(await consultasRegistroPoa.ListarPaginado(registroPoaFiltroDto));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.RPOA_I, Policy = "Bearer")]
        public async Task<IActionResult> Post([FromBody]RegistroPoaDto registroPoaDto, [FromServices]IComandosRegistroPoa comandosRegistroPoa)
        {
            await comandosRegistroPoa.Cadastrar(registroPoaDto);

            return Ok();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.RPOA_C, Policy = "Bearer")]
        public async Task<IActionResult> Put(long id, [FromBody]RegistroPoaDto registroPoaDto, [FromServices]IComandosRegistroPoa comandosRegistroPoa)
        {
            registroPoaDto.Id = id;

            await comandosRegistroPoa.Atualizar(registroPoaDto);

            return Ok();
        }
    }
}