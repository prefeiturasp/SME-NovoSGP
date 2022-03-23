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
    [Route("api/v1/calendarios/feriados")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class FeriadoCalendarioController : ControllerBase
    {
        private readonly IComandosFeriadoCalendario comandos;
        private readonly IConsultasFeriadoCalendario consultas;

        public FeriadoCalendarioController(IConsultasFeriadoCalendario consultas,
            IComandosFeriadoCalendario comandos)
        {
            this.consultas = consultas ?? throw new System.ArgumentNullException(nameof(consultas));
            this.comandos = comandos ?? throw new System.ArgumentNullException(nameof(comandos));
        }

        [HttpGet]
        [ProducesResponseType(typeof(FeriadoCalendarioCompletoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("{id}")]
        [Permissao(Permissao.TF_C, Policy = "Bearer")]
        public IActionResult BuscarPorId(long id)
        {
            return Ok(consultas.BuscarPorId(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<FeriadoCalendarioDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("listar")]
        [Permissao(Permissao.TF_C,Permissao.E_A,Permissao.E_C, Policy = "Bearer")]
        public async Task<IActionResult> BuscarTodos([FromBody] FiltroFeriadoCalendarioDto filtro)
        {
            var retorno = await consultas.Listar(filtro);
            if (retorno.Any())
                return Ok(retorno);
            else return StatusCode(204);
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.TF_E, Policy = "Bearer")]
        public IActionResult MarcarExcluidos([FromBody]long[] ids)
        {
            comandos.MarcarExcluidos(ids);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EnumeradoRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("abrangencias")]
        [Permissao(Permissao.TF_C, Policy = "Bearer")]
        public IActionResult ObterAbrangencias()
        {
            return Ok(consultas.ObterAbrangencias());
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EnumeradoRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("tipos")]
        [Permissao(Permissao.TF_C, Policy = "Bearer")]
        public IActionResult ObterTipos()
        {
            return Ok(consultas.ObterTipos());
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.TF_I, Permissao.TF_A, Policy = "Bearer")]
        public IActionResult Salvar([FromBody]FeriadoCalendarioDto dto)
        {
            comandos.Salvar(dto);
            return Ok();
        }
    }
}