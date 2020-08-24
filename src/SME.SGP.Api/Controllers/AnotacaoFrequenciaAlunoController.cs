using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/anotacoes/alunos")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class AnotacaoFrequenciaAlunoController : ControllerBase
    {
        [HttpGet("{codigoAluno}/aulas/{aulaId}")]
        [ProducesResponseType(typeof(AnotacaoFrequenciaAlunoDto), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PDA_C, Permissao.PDA_I, Permissao.PDA_A, Permissao.PDA_E, Policy = "Bearer")]
        public async Task<IActionResult> BuscarPorId(string codigoAluno, long aulaId, [FromServices] IObterAnotacaoFrequenciaAlunoUseCase useCase)
        {
            var anotacao = await useCase.Executar(new FiltroAnotacaoFrequenciaAlunoDto(codigoAluno, aulaId));

            if (anotacao == null)
                return NoContent();

            return Ok(anotacao);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PDA_I, Permissao.PDA_A, Permissao.PDA_E, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromBody] SalvarAnotacaoFrequenciaAlunoDto dto, [FromServices] ISalvarAnotacaoFrequenciaAlunoUseCase useCase)
        {
            return Ok(await useCase.Executar(dto));
        }
    }
}