using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/turmas")]
    public class TurmaController : ControllerBase
    {
        [HttpGet("{turmaCodigo}/alunos/anos/{anoLetivo}")]
        [ProducesResponseType(typeof(IEnumerable<AlunoDadosBasicosDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterAlunosTurma(string turmaCodigo, int anoLetivo, [FromServices] IConsultasTurma consultas)
           => Ok(await consultas.ObterDadosAlunos(turmaCodigo, anoLetivo));
    }
}
