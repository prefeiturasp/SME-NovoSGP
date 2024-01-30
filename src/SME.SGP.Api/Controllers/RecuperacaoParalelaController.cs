using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/recuperacao-paralela/")]
    [ValidaDto]
    [Authorize("Bearer")]
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
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_C, Policy = "Bearer")]
        public async Task<IActionResult> Listar([FromQuery]FiltroRecuperacaoParalelaDto filtro)
        {
            return Ok(await consultaRecuperacaoParalela.Listar(filtro));
        }

        [HttpGet("total-estudantes")]
        [ProducesResponseType(typeof(IEnumerable<RecuperacaoParalelaTotalEstudanteDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_C, Policy = "Bearer")]
        public async Task<IActionResult> ListarTotalEstudantes([FromQuery]FiltroRecuperacaoParalelaResumoDto filtro)
        {
            return Ok(await consultaRecuperacaoParalela.TotalEstudantes(filtro));
        }

        [HttpGet("grafico/frequencia")]
        [ProducesResponseType(typeof(IEnumerable<RecuperacaoParalelaTotalEstudantePorFrequenciaDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_C, Policy = "Bearer")]
        public async Task<IActionResult> ListarTotalEstudantesPorFrequencia([FromQuery]FiltroRecuperacaoParalelaResumoDto filtro)
        {
            return Ok(await consultaRecuperacaoParalela.TotalEstudantesPorFrequencia(filtro));
        }

        [HttpGet("resultado")]
        [ProducesResponseType(typeof(IEnumerable<RecuperacaoParalelaTotalResultadoDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_C, Policy = "Bearer")]
        public async Task<IActionResult> ListarTotalResultado([FromQuery]FiltroRecuperacaoParalelaResumoDto filtro)
        {
            return Ok(await consultaRecuperacaoParalela.ListarTotalResultado(filtro));
        }

        [HttpGet("resultado/encaminhamento")]
        [ProducesResponseType(typeof(IEnumerable<RecuperacaoParalelaTotalResultadoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_C, Policy = "Bearer")]
        public async Task<IActionResult> ListarTotalResultadoEncaminhamento([FromQuery]FiltroRecuperacaoParalelaResumoDto filtro)
        {
            return Ok(await consultaRecuperacaoParalela.ListarTotalResultadoEncaminhamento(filtro));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_I, Policy = "Bearer")]
        public async Task<IActionResult> PostAsync([FromBody]RecuperacaoParalelaDto recuperacaoParalelaPeriodoDto)
        {
            return Ok(await comandosRecuperacaoParalela.Salvar(recuperacaoParalelaPeriodoDto));
        }

        [HttpGet("periodo/{codigoTurma}/listar")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<RecuperacaoParalelaPeriodoPAPDto>),200)]
        [Permissao(Permissao.CP_C, Policy = "Bearer")]
        public async Task<IActionResult> GetListaPeriodoPAP(string codigoTurma, [FromServices]IConsultaRecuperacaoParalelaPeriodo consultaRecuperacaoParalelaPeriodo)
        {
            var retorno = await consultaRecuperacaoParalelaPeriodo.BuscarListaPeriodos(codigoTurma);

            if (retorno.EhNulo() || !retorno.Any())
                return NoContent();

            return Ok(retorno);
        }

        [HttpGet("anos-letivos")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<RecuperacaoParalelaPeriodoPAPDto>), 200)]
        [Permissao(Permissao.CP_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterAnosLetivos([FromServices]IObterAnosLetivosPAPUseCase obterAnosLetivosPAPUseCase)
        {
            var retorno = await obterAnosLetivosPAPUseCase.Executar();

            if (retorno.EhNulo() || !retorno.Any())
                return NoContent();

            return Ok(retorno);
        }

    }
}