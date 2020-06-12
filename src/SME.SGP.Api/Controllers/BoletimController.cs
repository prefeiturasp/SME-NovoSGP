using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/boletim")]
    public class BoletimController : ControllerBase
    {
        [HttpGet("imprimir")]
        public async Task<IActionResult> Imprimir([FromQuery] FiltroRelatorioBoletimDto filtroRelatorioBoletimDto, [FromServices] IBoletimUseCase boletimUseCase)
        {
            return Ok(await boletimUseCase.Executar(filtroRelatorioBoletimDto));
        }

        [HttpGet("alunos")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IEnumerable<AlunoSimplesDto>), 500)]
        public async Task<IActionResult> ListarAlunos([FromQuery] FiltroListagemAlunosDto filtroListagemAlunosDto, [FromServices] IObterListaAlunosDaTurmaUseCase obterListaAlunosDaTurmaUseCase)
        {

            return Ok(new List<AlunoSimplesDto>()
            {
                new AlunoSimplesDto() { NumeroChamada = 1, Nome = "Alvaro Ramos Grassi"},
                new AlunoSimplesDto() { NumeroChamada = 1, Nome = "Aline Grassi"},
                new AlunoSimplesDto() { NumeroChamada = 1, Nome = "Bianca Grassi"},
                new AlunoSimplesDto() { NumeroChamada = 1, Nome = "José Ramos Grassi"},
                new AlunoSimplesDto() { NumeroChamada = 1, Nome = "Valentina Grassi"},
                new AlunoSimplesDto() { NumeroChamada = 1, Nome = "Laura Ramos Grassi"},
                new AlunoSimplesDto() { NumeroChamada = 1, Nome = "Angela Ramos Grassi"},
                new AlunoSimplesDto() { NumeroChamada = 1, Nome = "Marcos Ramos Grassi"},
            });
        }
    }
}
