using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/boletim")]
    public class BoletimController : ControllerBase
    {
        [HttpGet("imprimir")]
        public async Task<IActionResult> Imprimir([FromQuery] FiltroRelatorioBoletimDto filtroRelatorioBoletimDto, [FromServices] IBoletimUseCase boletimUseCase)
        {
            return Ok(await boletimUseCase.Executar(filtroRelatorioBoletimDto));
        }

        [HttpGet("alunos")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<AlunoSimplesDto>), 500)]
        public async Task<IActionResult> ListarAlunos([FromQuery] string turmaCodigo, [FromServices] IObterListaAlunosDaTurmaUseCase obterListaAlunosDaTurmaUseCase)
        {
            return Ok(await obterListaAlunosDaTurmaUseCase.Executar(turmaCodigo));
        }
    }
}
