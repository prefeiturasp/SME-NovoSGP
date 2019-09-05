using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Api.Controllers
{
    [Route("api/v1/professores")]
    [ApiController]
    [ValidaDto]
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
    }
}