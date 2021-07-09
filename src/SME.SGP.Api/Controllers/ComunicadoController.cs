using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.Anos;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.EscolaAqui.Anos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/comunicado")]
    [Authorize("Bearer")]
    public class ComunicadoController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CO_I, Policy = "Bearer")]
        public async Task<IActionResult> PostAsync([FromBody] ComunicadoInserirDto comunicadoDto, [FromServices] ISolicitarInclusaoComunicadoEscolaAquiUseCase useCase)
        {
            return Ok(await useCase.Executar(comunicadoDto));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CO_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar(long id, [FromBody] ComunicadoAlterarDto comunicadoDto, [FromServices] ISolicitarAlteracaoComunicadoEscolaAquiUseCase useCase)
        {
            return Ok(await useCase.Executar(id, comunicadoDto));
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.CO_E, Policy = "Bearer")]
        public async Task<IActionResult> Excluir([FromBody] long[] ids, [FromServices] ISolicitarExclusaoComunicadosEscolaAquiUseCase useCase)
        {
            return Ok(await useCase.Executar(ids));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ComunicadoCompletoDto), 200)]
        [ProducesResponseType(typeof(IEnumerable<ComunicadoCompletoDto>), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CO_C, Policy = "Bearer")]
        public async Task<IActionResult> BuscarPorId(long id, [FromServices] IObterComunicadoEscolaAquiUseCase obterComunicadoEscolaAquiUseCase)
        {
            return Ok(await obterComunicadoEscolaAquiUseCase.Executar(id));
        }

        [HttpGet("{codigoTurma}/alunos/{anoLetivo}")]
        [ProducesResponseType(typeof(IEnumerable<AlunoPorTurmaResposta>), 200)]
        [ProducesResponseType(typeof(IEnumerable<AlunoPorTurmaResposta>), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> BuscarAlunos(string codigoTurma, int anoLetivo, [FromServices] IObterAlunosPorTurmaEAnoLetivoEscolaAquiUseCase obterAlunosPorTurmaEscolaAquiUseCase)
        {
            var retorno = await obterAlunosPorTurmaEscolaAquiUseCase.Executar(codigoTurma, anoLetivo);
            if (retorno == null || !retorno.Any())
                return NoContent();

            return Ok(retorno);
        }

        [HttpGet("listar")]
        [ProducesResponseType(typeof(IEnumerable<ComunicadoCompletoDto>), 200)]
        [ProducesResponseType(typeof(IEnumerable<ComunicadoCompletoDto>), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CO_C, Policy = "Bearer")]
        public async Task<IActionResult> BuscarTodosAsync([FromQuery] FiltroComunicadoDto filtro, [FromServices] IObterComunicadosPaginadosEscolaAquiUseCase obterComunicadosPaginadosEscolaAquiUseCase)
        {
            var resultado = await obterComunicadosPaginadosEscolaAquiUseCase.Executar(filtro);

            if (!resultado.Items.Any())
                return NoContent();

            return Ok(resultado);
        }

        [HttpGet("anos/modalidade/{modalidade}")]
        [ProducesResponseType(typeof(IEnumerable<AnosPorCodigoUeModalidadeEscolaAquiResult>), 200)]
        [ProducesResponseType(typeof(IEnumerable<AnosPorCodigoUeModalidadeEscolaAquiResult>), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CO_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterAnosPorCodigoUeModalidade(Modalidade modalidade, [FromQuery] string codigoUe, [FromServices] IObterAnosPorCodigoUeModalidadeEscolaAquiUseCase obterAnosPorCodigoUeModalidadeEscolaAquiUseCase)
        {
            var resultado = await obterAnosPorCodigoUeModalidadeEscolaAquiUseCase.Executar(codigoUe, modalidade);

            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }

        [HttpGet("turmas/{turmaId}/semestres/{semestre}/alunos/{alunoCodigo}")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>), 200)]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterComunicadosDoAluno(long turmaId, int semestre, long alunoCodigo, [FromServices] IObterComunicadosPaginadosAlunoUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroTurmaAlunoSemestreDto(turmaId, alunoCodigo, semestre)));
        }
    }
}