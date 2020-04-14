using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/conselhos-classe")]
    [Authorize("Bearer")]
    public class ConselhoClasseController : ControllerBase
    {
        [HttpGet("recomendacoes/turmas/{codigoTurma}/alunos/{codigoAluno}/bimestres/{numeroBimestre}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(ConsultasConselhoClasseRecomendacaoConsultaDto), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterRecomendacoesAlunoFamilia([FromServices]IConsultasConselhoClasseRecomendacao consultasConselhoClasseRecomendacao,
            string codigoTurma, string codigoAluno, int numeroBimestre, [FromQuery]Modalidade modalidade, [FromQuery]bool EhFinal = false)
        {
            var retorno = await consultasConselhoClasseRecomendacao.ObterRecomendacoesAlunoFamilia(codigoTurma, codigoAluno, numeroBimestre, modalidade, EhFinal);
            return Ok(retorno);
        }

        [HttpPost("pos-conselhos/notas")]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        [Permissao(Permissao.CC_I, Policy = "Bearer")]
        public async Task<IActionResult> PersistirNotas([FromServices]IConsultasConselhoClasseRecomendacao consultasConselhoClasseRecomendacao,
           [FromBody]PosConselhosNotasPersistirDto posConselhosNotasPersistirDto)
        {
            return Ok();
        }
    }
}