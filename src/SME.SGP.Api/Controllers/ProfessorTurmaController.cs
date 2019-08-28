using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;

namespace SME.SGP.Api.Controllers
{
    [Route("api/v1/professor-turma")]
    [ApiController]
    [ValidaDto]
    public class ProfessorTurmaController : ControllerBase
    {
        private readonly IConsultasProfessorTurma consultasProfessorTurma;

        public ProfessorTurmaController(IConsultasProfessorTurma consultasProfessorTurma)
        {
            this.consultasProfessorTurma = consultasProfessorTurma;
        }

        [HttpGet]
        [Route("{codigoRf}/turmas")]
        public IActionResult Get(string codigoRf)
        {
            return Ok(consultasProfessorTurma.Listar(codigoRf));
        }
    }
}