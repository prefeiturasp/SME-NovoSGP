using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/acompanhamento/alunos")]
    [ValidaDto]
    public class AcompanhamentoAlunoController : Controller
    {
        [HttpPost("semestres")]
        [ProducesResponseType(typeof(IEnumerable<SinteseDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Salvar([FromServices] ISalvarAcompanhamentoAlunoUseCase useCase, [FromBody] AcompanhamentoAlunoDto dto)
             => Ok(await useCase.Executar(dto));

        [HttpPost("semestres/upload")]
        [ProducesResponseType(typeof(IEnumerable<SinteseDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 400)]
        public async Task<IActionResult> UploadFoto([FromForm] IFormFile file, [FromBody] AcompanhamentoAlunoDto dto, [FromServices] ISalvarFotoAlunoUseCase useCase)
        {
            if (file.Length > 0)
                Ok(await useCase.Executar(dto, file));

            return BadRequest();
        }
    }
}