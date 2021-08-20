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
            var retorno = consultasProfessor.Listar(codigoRf);

            return Ok(retorno);
        }

        [HttpGet("{codigoRF}/escolas/{codigoEscola}/turmas/anos-letivos/{anoLetivo}")]
        [ProducesResponseType(typeof(IEnumerable<DisciplinaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Get(string codigoRF, string codigoEscola, int anoLetivo, [FromServices]IConsultasProfessor consultasProfessor)
        {
            var retorno = await consultasProfessor.ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(codigoRF, codigoEscola, anoLetivo);

            return Ok(retorno);
        }

        [HttpGet("turmas/{codigoTurma}/disciplinas/agrupadas")]
        [ProducesResponseType(typeof(IEnumerable<DisciplinaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> GetDisciplinasAgrupadas(string codigoTurma, [FromQuery] bool turmaPrograma, [FromServices]IConsultasDisciplina consultasDisciplina)
        {
            var retorno = await consultasDisciplina.ObterDisciplinasAgrupadasPorProfessorETurma(codigoTurma, turmaPrograma);

            return Ok(retorno);
        }

        [HttpGet("turmas/{codigoTurma}/disciplinas/")]
        [ProducesResponseType(typeof(IEnumerable<DisciplinaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterDisciplinas(string codigoTurma, [FromQuery] bool turmaPrograma, [FromServices]IConsultasDisciplina consultasDisciplina)
        {
            var retorno = await consultasDisciplina.ObterComponentesCurricularesPorProfessorETurma(codigoTurma, turmaPrograma);

            return Ok(retorno);
        }

        [HttpPost("disciplinas/turmas")]
        [ProducesResponseType(typeof(IEnumerable<DisciplinaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterDisciplinas([FromBody] string[] codigosTurmas, [FromServices]IObterComponentesCurricularesPorProfessorETurmasCodigosUseCase useCase)
        {
            var retorno = await useCase.Executar(codigosTurmas);

            return Ok(retorno);
        }

        [HttpGet("turmas/{codigoTurma}/docencias-compartilhadas/disciplinas/")]
        [ProducesResponseType(typeof(IEnumerable<DisciplinaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterDisciplinasCompartilhadas(string codigoTurma, [FromQuery] bool turmaPrograma, [FromServices]IConsultasDisciplina consultasDisciplina)
        {
            return Ok(await consultasDisciplina.ObterDisciplinasPorTurma(codigoTurma, turmaPrograma));
        }

        [HttpGet("turmas/{codigoTurma}/disciplinas/planejamento")]
        [ProducesResponseType(typeof(IEnumerable<DisciplinaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_I, Permissao.PA_A, Permissao.PA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterDisciplinasParaPlanejamento(string codigoTurma, long codigoDisciplina, bool turmaPrograma, bool regencia, [FromServices]IConsultasDisciplina consultasDisciplina)
        {
            var retorno = await consultasDisciplina.ObterComponentesCurricularesPorProfessorETurmaParaPlanejamento(codigoDisciplina, codigoTurma, turmaPrograma, regencia);

            return Ok(retorno);
        }

        [HttpGet("{codigoRF}/resumo/{anoLetivo}")]
        [ProducesResponseType(typeof(ProfessorResumoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Resumo(string codigoRF, int anoLetivo,[FromQuery] bool buscarOutrosCargos = false)
        {
            var retorno = await consultasProfessor.ObterResumoPorRFAnoLetivo(codigoRF, anoLetivo,buscarOutrosCargos);

            return Ok(retorno);
        }

        [HttpGet("{codigoRF}/consultarprofessorrf/{anoLetivo}")]
        [ProducesResponseType(typeof(ProfessorResumoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ConsultarProfessorRf(string codigoRF, int anoLetivo,string dreId,string ueId)
        {
            var retorno = await consultasProfessor.ObterResumoPorRFUeDreAnoLetivo(codigoRF, anoLetivo, dreId,ueId);

            return Ok(retorno);
        }

        [HttpGet("{anoLetivo}/autocomplete/{dreId}")]
        [ProducesResponseType(typeof(IEnumerable<ProfessorResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ResumoAutoComplete(int anoLetivo, string dreId, string ueId, string nomeProfessor)
        {
            var retorno = await consultasProfessor.ObterResumoAutoComplete(anoLetivo, dreId, ueId,nomeProfessor);

            return Ok(retorno);
        }
    }
}