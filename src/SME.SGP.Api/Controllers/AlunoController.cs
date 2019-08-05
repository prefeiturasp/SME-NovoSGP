using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/alunos")]
    [ValidaDto]
    public class AlunoController : ControllerBase
    {
        private readonly IManterAluno manterAluno;

        public AlunoController(IManterAluno manterAluno)
        {
            this.manterAluno = manterAluno ?? throw new System.ArgumentNullException(nameof(manterAluno));
        }

        [HttpGet]
        public IActionResult Get(int pagina, int tamanho)
        {
            return Ok(manterAluno.Listar(pagina,tamanho));
        }

        [HttpPost]
        public IActionResult Post(AlunoDto alunoDto)
        {
            manterAluno.SalvarCriandoProfessor(alunoDto);
            return Ok();
        }
    }
}