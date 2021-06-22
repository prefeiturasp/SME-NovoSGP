using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/calendarios/eventos")]
    [Authorize("Bearer")]
    public class EventoController : ControllerBase
    {
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.E_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar(long id, [FromBody]EventoDto eventoDto, [FromServices]IComandosEvento comandosEvento)
        {
            return Ok(await comandosEvento.Alterar(id, eventoDto));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.E_I, Policy = "Bearer")]
        public async Task<IActionResult> Criar([FromServices]IComandosEvento comandosEvento, [FromBody]EventoDto eventoDto)
        {
            return Ok(await comandosEvento.Criar(eventoDto));
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.E_E, Policy = "Bearer")]
        public async Task<IActionResult> Excluir(long[] eventosId, [FromServices]IComandosEvento comandosEvento)
        {
            await comandosEvento.Excluir(eventosId);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.E_C, Policy = "Bearer")]
        public async Task<IActionResult> Listar([FromQuery]FiltroEventosDto filtroEventosDto, [FromServices] IConsultasEvento consultasEvento)
        {
            return Ok(await consultasEvento.Listar(filtroEventosDto));
        }

        [HttpGet("liberacao-boletim/bimestres")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.E_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterBimestresLiberacaoBoletim([FromServices] IObterBimestresLiberacaoBoletimUseCase obterBimestresLiberacaoBoletimUseCase)
        {
            return Ok(await obterBimestresLiberacaoBoletimUseCase.Executar());
        }

        [HttpGet("meses")]
        [ProducesResponseType(typeof(IEnumerable<CalendarioEventosMesesDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.E_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterMeses([FromServices] IConsultasEvento consultasEvento,
                            [FromQuery]CalendarioEventosFiltroDto calendarioEventoMesesFiltro)

        {
            var retorno = await consultasEvento.ObterQuantidadeDeEventosPorMeses(calendarioEventoMesesFiltro);
            if (retorno.Any())
                return Ok(retorno);
            else return StatusCode(204);
        }

        [HttpGet("meses/{mes}/dias/{dia}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(IEnumerable<CalendarioTipoEventoPorDiaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.E_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPorDia(int dia, int mes, [FromQuery]CalendarioEventosFiltroDto filtro, [FromServices] IConsultasEvento consultasEvento)
        {
            var retorno = await consultasEvento.ObterEventosPorDia(filtro, mes, dia);
            if (retorno.Any())
                return Ok(retorno);
            else return StatusCode(204);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EventoCompletoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.E_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPorId(long id, [FromServices] IConsultasEvento consultasEvento)
        {
            return Ok(await consultasEvento.ObterPorId(id));
        }

        [HttpGet("meses/{mes}/tipos")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(IEnumerable<CalendarioTipoEventoPorDiaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.E_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPorMes([FromQuery]CalendarioEventosFiltroDto filtro, int mes, [FromServices]IConsultasEvento consultasEvento)
        {
            var listaRetorno = await consultasEvento.ObterQuantidadeDeEventosPorDia(filtro, mes);

            if (listaRetorno.Any())
                return Ok(listaRetorno);
            else return StatusCode(204);
        }
    }
}