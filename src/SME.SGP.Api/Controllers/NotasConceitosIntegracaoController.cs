using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/avaliacoes/notas/integracoes")]
    [ChaveIntegracaoSgpApi]
    public class NotasConceitosIntegracaoController : ControllerBase
    {
        [HttpGet("ues/{ueCodigo}/turmas/{turmaCodigo}/alunos/{alunoCodigo}")]
        [ProducesResponseType(typeof(IEnumerable<NotaConceitoBimestreComponenteDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ChaveIntegracaoSgpApi]
        public async Task<IActionResult> ObterNotasPorBimestresUeAlunoTurma(string ueCodigo, string turmaCodigo, string alunoCodigo, [FromQuery] int[] bimestres, [FromServices] IObterNotasPorBimestresUeAlunoTurmaUseCase obterNotasPorBimestresUeAlunoTurmaUseCase)
        {
            return Ok(await obterNotasPorBimestresUeAlunoTurmaUseCase.Executar(new NotaConceitoPorBimestresAlunoTurmaDto(ueCodigo, turmaCodigo, alunoCodigo, bimestres)));
        }
    }
}
