using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/fechamentos/turmas")]
    public class FechamentoTurmaController : ControllerBase
    {
        [HttpPost()]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.CP_I, Policy = "Bearer")]
        public async Task<IActionResult> Inserir([FromBody] IEnumerable<FechamentoTurmaDisciplinaDto> fechamentoTurma, [FromServices] IComandosFechamentoTurmaDisciplina comandos)
            => Ok(await comandos.Inserir(fechamentoTurma));

        [HttpPut()]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.CP_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromBody] IEnumerable<FechamentoTurmaDisciplinaDto> fechamentoTurma, [FromServices] IComandosFechamentoTurmaDisciplina comandos)
            => Ok(await comandos.Alterar(fechamentoTurma));
    }
}