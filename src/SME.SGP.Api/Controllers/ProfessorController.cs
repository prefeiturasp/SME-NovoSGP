using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [Route("api/v1/professores")]
    [ApiController]
    [ValidaDto]
    [Authorize("Bearer")]
    public class ProfessorController : ControllerBase
    {
        private readonly IConsultasProfessor consultasProfessor;

        public ProfessorController(IConsultasProfessor consultasProfessor)
        {
            this.consultasProfessor = consultasProfessor;
        }

        [HttpGet]
        [Route("{codigoRf}/turmas")]
        [ProducesResponseType(typeof(IEnumerable<ProfessorTurmaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Get(string codigoRf)
        {
            return Ok(consultasProfessor.Listar(codigoRf));
        }

        [HttpGet("{codigoRF}/turmas/{codigoTurma}/disciplinas/")]
        [ProducesResponseType(typeof(IEnumerable<DisciplinaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Get(long codigoTurma, string codigoRF, [FromServices]IConsultasDisciplina consultasDisciplina)
        {
            return Ok(await consultasDisciplina.ObterDisciplinasPorProfessorETurma(codigoTurma));
        }

        [HttpGet("{codigoRF}/escolas/{codigoEscola}/turmas/anos-letivos/{anoLetivo}")]
        [ProducesResponseType(typeof(IEnumerable<DisciplinaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Get(string codigoRF, string codigoEscola, int anoLetivo, [FromServices]IConsultasProfessor consultasProfessor)
        {
            return Ok(await consultasProfessor.ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(codigoRF, codigoEscola, anoLetivo));
        }

        [HttpGet("turmas/{codigoTurma}/disciplinas/planejamento")]
        [ProducesResponseType(typeof(IEnumerable<DisciplinaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_I, Permissao.PA_A, Permissao.PA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterDisciplinasParaPlanejamento(long codigoTurma, [FromServices]IConsultasDisciplina consultasDisciplina)
        {
            return Ok(await consultasDisciplina.ObterDisciplinasParaPlanejamento(codigoTurma));
        }
    }
}