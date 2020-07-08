using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using SME.SGP.Infra.Dtos.Relatorios.HistoricoEscolar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlunoDto = SME.SGP.Infra.Dtos.Relatorios.HistoricoEscolar.AlunoDto;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/historico-escolar")]
    public class HistoricoEscolarController : ControllerBase
    {

        [HttpPost]
        [Route("alunos")]
        [ProducesResponseType(typeof(IEnumerable<AlunoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public IActionResult ObterAlunos(FiltroBuscaAlunosDto filtroBuscaAlunosDto)
        {
            List<AlunoDto> alunos = new List<AlunoDto>
            {
                new AlunoDto("6588992", "ANA VIEIRA ALMEIDA"),
                new AlunoDto("6249873", "ANA CLARA DO PRADO MARTINS"),
                new AlunoDto("6145218", "ANA JULIA DOS SANTOS SOUZA"),
                new AlunoDto("6233047", "ANA HENRIQUE PRIORI GOMES")
            };
            return Ok(alunos);
        }


        [HttpPost]
        [Route("gerar")]
        [ProducesResponseType(typeof(Boolean), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> Gerar(FiltroHistoricoEscolarDto filtroHistoricoEscolarDto, [FromServices] IHistoricoEscolarUseCase historicoEscolarUseCase)
        {
            return Ok(await historicoEscolarUseCase.Executar(filtroHistoricoEscolarDto));
        }

    }
}