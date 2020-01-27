using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/recuperacao-paralela/")]
    [ValidaDto]
    public class RecuperacaoParalelaController : ControllerBase
    {
        private readonly IComandosRecuperacaoParalela comandosRecuperacaoParalela;
        private readonly IConsultaRecuperacaoParalela consultaRecuperacaoParalela;

        public RecuperacaoParalelaController(IConsultaRecuperacaoParalela consultaRecuperacaoParalela, IComandosRecuperacaoParalela comandosRecuperacaoParalela)
        {
            this.consultaRecuperacaoParalela = consultaRecuperacaoParalela ?? throw new System.ArgumentNullException(nameof(consultaRecuperacaoParalela));
            this.comandosRecuperacaoParalela = comandosRecuperacaoParalela ?? throw new System.ArgumentNullException(nameof(comandosRecuperacaoParalela));
        }

        [HttpGet("listar")]
        [ProducesResponseType(typeof(IEnumerable<RecuperacaoParalelaListagemDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Listar([FromQuery]FiltroRecuperacaoParalelaDto filtro)
        {
            return Ok(await consultaRecuperacaoParalela.Listar(filtro));
        }

        //[HttpGet("listar-periodos")]
        //[ProducesResponseType(typeof(IEnumerable<RecuperacaoParalelaDto>), 200)]
        //[ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //public async Task<IActionResult> ListarPeriodos()
        //{
        //    return Ok(await consultaRecuperacaoParalela.ListarPeriodo());
        //}

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.CP_I, Policy = "Bearer")]
        public async Task<IActionResult> PostAsync([FromBody]RecuperacaoParalelaDto recuperacaoParalelaPeriodoDto)
        {
            return Ok(await comandosRecuperacaoParalela.Salvar(recuperacaoParalelaPeriodoDto));
        }
    }
}