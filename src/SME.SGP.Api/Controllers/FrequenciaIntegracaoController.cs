using MediatR;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/calendarios/frequencias/integracoes/")]
    [ChaveIntegracaoSgpApi]
    public class FrequenciaIntegracaoController : ControllerBase
    {
        private readonly IMediator mediator;

        public FrequenciaIntegracaoController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("alunos/{alunoCodigo}/turmas/{turmaCodigo}/geral")]
        [ChaveIntegracaoSgpApi]
        [ProducesResponseType(typeof(double), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterFrequenciaGeralAluno(string alunoCodigo, string turmaCodigo)
            => Ok(await mediator.Send(new ObterConsultaFrequenciaGeralAlunoQuery(alunoCodigo, turmaCodigo)));

        [HttpGet("turmas/{turmaCodigo}/alunos/{alunoCodigo}/componentes-curriculares/{componenteCurricularId}")]
        [ProducesResponseType(typeof(IEnumerable<FrequenciaAluno>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ChaveIntegracaoSgpApi]
        public async Task<IActionResult> ObterFrequenciasPorBimestresAlunoTurmaComponenteCurricular(string turmaCodigo, string alunoCodigo, string componenteCurricularId, [FromQuery] int[] bimestres, [FromServices] IObterFrequenciasPorBimestresAlunoTurmaComponenteCurricularUseCase useCase)
        {
            return Ok(await useCase.Executar(new FrequenciaPorBimestresAlunoTurmaComponenteCurricularDto(turmaCodigo, alunoCodigo, bimestres, componenteCurricularId)));
        }
    }
}
