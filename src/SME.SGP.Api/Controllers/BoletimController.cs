using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/boletim")]
    [Authorize("Bearer")]
    public class BoletimController : ControllerBase
    {
        [HttpPost("imprimir")]
        [Permissao(Permissao.B_C, Policy = "Bearer")]
        public async Task<IActionResult> Imprimir([FromBody] FiltroRelatorioBoletimDto filtroRelatorioBoletimDto, [FromServices] IBoletimUseCase boletimUseCase)
        {
            return Ok(await boletimUseCase.Executar(filtroRelatorioBoletimDto));
        }

        [HttpGet("alunos")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<AlunoSimplesDto>), 500)]
        [Permissao(Permissao.B_C, Policy = "Bearer")]
        public async Task<IActionResult> ListarAlunos([FromQuery] string turmaCodigo, [FromServices] IObterListaAlunosDaTurmaUseCase obterListaAlunosDaTurmaUseCase)
        {
            return Ok(await obterListaAlunosDaTurmaUseCase.Executar(turmaCodigo));
        }

        [HttpGet("alunos-obsevacoes")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<AlunoComObservacaoDoHistoricoEscolarDto>), 500)]
        [Permissao(Permissao.B_C, Policy = "Bearer")]
        public async Task<IActionResult> ListarAlunos([FromQuery] string turmaCodigo, [FromServices] IObterObservacoesDosAlunosNoHistoricoEscolarUseCase obterListaAlunosDaTurmaUseCase)
        {
            return Ok(await obterListaAlunosDaTurmaUseCase.Executar(turmaCodigo));
        }

        
    }
}
