using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/acompanhamento-frequencia")]
    [Authorize("Bearer")]
    public class AcompanhamentoFrequenciaController : ControllerBase
    {
        [HttpGet("")]
        [ProducesResponseType(typeof(FrequenciaAlunosPorBimestreDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 602)]
        public async Task<IActionResult> ObterInformacoesDeFrequenciaAlunosPorBimestre([FromQuery] ObterFrequenciaAlunosPorBimestreDto dto)
        {
            var frequenciaAlunos = new List<AlunoFrequenciaDto>
            {
                new AlunoFrequenciaDto
                {
                    AlunoRf = 999999,
                    Ausencias = 10,
                    Compensacoes = 3,
                    Frequencia = 93.0m,
                    Nome = "Carlos Dias",
                    NumeroChamada = 1,
                    PossuiJustificativas = false,
                    MarcadorFrequencia = new MarcadorFrequenciaDto { Tipo = Dominio.TipoMarcadorFrequencia.Novo, Descricao = "Estudante Novo: Data da matrícula 06/01/2021" }
                },
                new AlunoFrequenciaDto
                {
                    AlunoRf = 888888,
                    Ausencias = 1,
                    Compensacoes = 0,
                    Frequencia = 99.0m,
                    Nome = "Marcos Lobo",
                    NumeroChamada = 2,
                    PossuiJustificativas = true
                },
                new AlunoFrequenciaDto
                {
                    AlunoRf = 77777,
                    Ausencias = 78,
                    Compensacoes = 8,
                    Frequencia = 30.0m,
                    Nome = "Jefferson Cassiano",
                    NumeroChamada = 3,
                    PossuiJustificativas = false
                },
            };

            var frequenciaBimestre = new FrequenciaAlunosPorBimestreDto
            {
                AulasDadas = 100,
                AulasPrevistas = 100,
                Bimestre = 1,
                FrequenciaAlunos = frequenciaAlunos,
                MostraColunaCompensacaoAusencia = true,
                MostraLabelAulasPrevistas = false
            };

            return Ok(await Task.FromResult(frequenciaBimestre));
        }

        [HttpGet("turmas/{turmaId}/componentes-curriculares/{componenteCurricularId}/alunos/{alunoCodigo}/justificativas")]
        [ProducesResponseType(typeof(IEnumerable<JustificativaAlunoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 602)]
        public async Task<IActionResult> ObterJustificativasAlunoPorComponenteCurricular(long turmaId, long alunoCodigo, long componenteCurricularId, [FromServices] IObterJustificativasAlunoPorComponenteCurricularUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroJustificativasAlunoPorComponenteCurricular(turmaId, componenteCurricularId, alunoCodigo)));
        }
    }
}
