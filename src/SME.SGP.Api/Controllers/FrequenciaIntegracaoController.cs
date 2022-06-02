using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [Route("api/v1/frequencias/integracoes")]
    [ApiController]
    [ValidaDto]
    [ChaveIntegracaoSgpApi]
    public class FrequenciaIntegracaoController : ControllerBase
    {
        [HttpGet("turmas/{turmaCodigo}/alunos/{alunoCodigo}/componentes-curriculares/{componenteCurricularId}")]
        [ProducesResponseType(typeof(IEnumerable<FrequenciaAluno>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterFrequenciasPorBimestresAlunoTurmaComponenteCurricular(string turmaCodigo, string alunoCodigo, string componenteCurricularId, [FromQuery] int[] bimestres, [FromServices] IObterFrequenciasPorBimestresAlunoTurmaComponenteCurricularUseCase useCase)
        {
            return Ok(await useCase.Executar(new FrequenciaPorBimestresAlunoTurmaComponenteCurricularDto(turmaCodigo, alunoCodigo, bimestres, componenteCurricularId)));
        }
    }
}
