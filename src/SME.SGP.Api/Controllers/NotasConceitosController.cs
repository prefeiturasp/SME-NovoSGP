using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/avaliacoes/notas")]
    [ValidaDto]
    public class NotasConceitosController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(NotasConceitosRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NC_C, Permissao.NC_I, Policy = "Bearer")]
        public async Task<IActionResult> Get([FromQuery] ListaNotasConceitosConsultaRefatoradaDto consultaListaNotasConceitosDto, [FromServices] IObterNotasParaAvaliacoesUseCase obterNotasParaAvaliacoesUseCase)
        {
            return Ok(await obterNotasParaAvaliacoesUseCase.Executar(consultaListaNotasConceitosDto));
        }
        [HttpGet("periodos")]
        [ProducesResponseType(typeof(IEnumerable<PeriodosParaConsultaNotasDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NC_C, Permissao.NC_I, Policy = "Bearer")]
        public async Task<IActionResult> ObterPeriodosParaConsulta([FromQuery] ObterPeriodosParaConsultaNotasFiltroDto filtro, [FromServices] IObterPeriodosParaConsultaNotasUseCase obterNotasParaAvaliacoesUseCase)
        {
            return Ok(await obterNotasParaAvaliacoesUseCase.Executar(filtro));
        }
        [HttpGet("/api/v1/avaliacoes/{atividadeAvaliativaId}/notas/{nota}/arredondamento")]
        [ProducesResponseType(typeof(double), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NC_C, Permissao.NC_I, Policy = "Bearer")]
        public async Task<IActionResult> ObterArredondamento(long atividadeAvaliativaId, double nota, [FromServices] IConsultasNotasConceitos consultasNotasConceitos)
        {
            return Ok(await consultasNotasConceitos.ObterValorArredondado(atividadeAvaliativaId, nota));
        }

        [HttpGet("{nota}/arredondamento")]
        [ProducesResponseType(typeof(double), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NC_C, Permissao.NC_I, Policy = "Bearer")]
        public async Task<IActionResult> ObterArredondamento(double nota, [FromQuery] DateTime data, [FromServices] IConsultasNotasConceitos consultasNotasConceitos)
        {
            return Ok(await consultasNotasConceitos.ObterValorArredondado(data, nota));
        }

        [HttpGet("turmas/{turmaId}/anos-letivos/{anoLetivo}/tipos")]
        [ProducesResponseType(typeof(TipoNota), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NC_C, Permissao.NC_I, Policy = "Bearer")]
        public async Task<IActionResult> ObterNotaTipo(long turmaId, int anoLetivo, [FromQuery] bool consideraHistorico, [FromServices] IConsultasNotasConceitos consultasNotasConceitos)
        {
            return Ok(await consultasNotasConceitos.ObterNotaTipo(turmaId, anoLetivo, consideraHistorico));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NC_A, Permissao.NC_I, Policy = "Bearer")]
        public async Task<IActionResult> Post([FromBody] NotaConceitoListaDto notaConceitoListaDto, [FromServices] IComandosNotasConceitos comandosNotasConceitos)
        {
            await comandosNotasConceitos.Salvar(notaConceitoListaDto);

            return Ok();
        }

        [HttpGet("conceitos")]
        [ProducesResponseType(typeof(IEnumerable<ConceitoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterConceitos([FromQuery] DateTime data, [FromServices] IConsultasNotasConceitos consultasNotasConceitos)
        {
            var listaConceitos = await consultasNotasConceitos.ObterConceitos(data);

            if (listaConceitos == null || !listaConceitos.Any())
                return NoContent();

            return Ok(listaConceitos);
        }

        [HttpGet("ues/{ueCodigo}/turmas/{turmaCodigo}/alunos/{alunoCodigo}")]
        [ProducesResponseType(typeof(IEnumerable<ConceitoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Authorize("Bearer")]
        public async Task<IActionResult> ObterNotasPorBimestresUeAlunoTurma(string ueCodigo, string turmaCodigo, string alunoCodigo, [FromQuery] int[] bimestres, [FromServices] IObterNotasPorBimestresUeAlunoTurmaUseCase useCase)
        {
            return Ok(await useCase.Executar(new NotaConceitoPorBimestresAlunoTurmaDto(ueCodigo, turmaCodigo, alunoCodigo, bimestres)));
        }

    }
}