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
    [Route("api/v1/calendarios/tipos")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class TipoCalendarioController : ControllerBase
    {
        private readonly IComandosTipoCalendario comandos;
        private readonly IConsultasTipoCalendario consultas;

        public TipoCalendarioController(IConsultasTipoCalendario consultas,
            IComandosTipoCalendario comandos)
        {
            this.consultas = consultas ?? throw new System.ArgumentNullException(nameof(consultas));
            this.comandos = comandos ?? throw new System.ArgumentNullException(nameof(comandos));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.TCE_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromBody]TipoCalendarioDto dto, long id)
        {
            await comandos.Alterar(dto, id);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TipoCalendarioDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.TCE_C, Permissao.E_C, Policy = "Bearer")]
        [Route("anos/letivos/{anoLetivo}")]
        public async Task<IActionResult> BuscarPorAnoLetivo(int anoLetivo)
        {
            var retorno = await consultas.ListarPorAnoLetivo(anoLetivo);

            if (retorno == null || !retorno.Any())
                return NoContent();

            return Ok(retorno);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TipoCalendarioDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.TCE_C, Permissao.E_C, Policy = "Bearer")]
        public async Task<IActionResult> BuscarTodos()
        {
            return Ok(await consultas.Listar());
        }

        [HttpGet]
        [ProducesResponseType(typeof(TipoCalendarioCompletoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("{id}")]
        [Permissao(Permissao.TCE_C, Permissao.E_C, Policy = "Bearer")]
        public async Task<IActionResult> BuscarUm(long id)
        {
            return Ok(await consultas.BuscarPorId(id));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.TCE_I, Policy = "Bearer")]
        public async Task<IActionResult> Incluir([FromBody]TipoCalendarioDto dto)
        {
            await comandos.Incluir(dto);
            return Ok();
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.TCE_E, Policy = "Bearer")]
        public IActionResult MarcarExcluidos([FromBody]long[] ids)
        {
            comandos.MarcarExcluidos(ids);
            return Ok();
        }
    }
}