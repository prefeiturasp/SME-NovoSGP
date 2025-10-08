using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
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
    [Route("api/v1/comunicados")]
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

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PaginacaoResultadoDto<ComunicadoListaPaginadaDto>>), 200)]
        [ProducesResponseType(typeof(IEnumerable<ComunicadoListaPaginadaDto>), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CO_C, Policy = "Bearer")]
        public async Task<IActionResult> ListarComunicados([FromQuery] FiltroComunicadoDto filtro, [FromServices] IObterComunicadosPaginadosEscolaAquiUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
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

        [HttpGet("{codigoTurma}/alunos/{AnoLetivo}")]
        [ProducesResponseType(typeof(IEnumerable<AlunoPorTurmaResposta>), 200)]
        [ProducesResponseType(typeof(IEnumerable<AlunoPorTurmaResposta>), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CO_C, Policy = "Bearer")]
        public async Task<IActionResult> BuscarAlunos(string codigoTurma, int anoLetivo, [FromServices] IObterAlunosPorTurmaEAnoLetivoEscolaAquiUseCase obterAlunosPorTurmaEscolaAquiUseCase)
        {
            var retorno = await obterAlunosPorTurmaEscolaAquiUseCase.Executar(codigoTurma, anoLetivo);
            if (retorno.EhNulo() || !retorno.Any())
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

        [HttpGet("anos/modalidades")]
        [ProducesResponseType(typeof(IEnumerable<AnosPorCodigoUeModalidadeEscolaAquiResult>), 200)]
        [ProducesResponseType(typeof(IEnumerable<AnosPorCodigoUeModalidadeEscolaAquiResult>), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CO_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterAnosPorCodigoUeModalidade([FromQuery] string codigoUe, [FromQuery] int[] modalidades, [FromServices] IObterAnosPorCodigoUeModalidadeEscolaAquiUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoUe, modalidades));
        }

        [HttpGet("turmas/{turmaId}/semestres/{semestre}/alunos/{alunoCodigo}")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>), 200)]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CO_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterComunicadosDoAluno(long turmaId, int semestre, long alunoCodigo, [FromServices] IObterComunicadosPaginadosAlunoUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroTurmaAlunoSemestreDto(turmaId, alunoCodigo, semestre)));
        }

        [HttpGet("anos-letivos")]
        [ProducesResponseType(typeof(IEnumerable<long>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CO_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterAnosLetivos([FromQuery] int anoMinimo, [FromServices] IObterAnosLetivosComunicadoUseCase useCase)
        {
            return Ok(await useCase.Executar(anoMinimo));
        }

        [HttpGet("ues/{codigoUe}/anoletivo/{AnoLetivo}/turmas")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<DropdownTurmaRetornoDto>), 200)]
        [Permissao(Permissao.CO_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolares(int anoLetivo, string codigoUe, [FromQuery] int semestre, [FromQuery] int[] modalidades, [FromQuery] string[] anos, [FromQuery] bool consideraHistorico, [FromServices] IObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo, codigoUe, modalidades, semestre, anos, consideraHistorico));
        }

        [HttpGet("semestres/consideraHistorico/{consideraHistorico}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<int>), 200)]
        [Permissao(Permissao.CO_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSemestres(bool consideraHistorico, [FromQuery] int modalidade, [FromQuery] int anoLetivo, [FromQuery] string ueCodigo, [FromServices] IObterSemestresPorAnoLetivoModalidadeEUeCodigoUseCase useCase)
        {
            return Ok(await useCase.Executar(consideraHistorico, modalidade, anoLetivo, ueCodigo));
        }

        [HttpGet("filtro/anos-letivos/{AnoLetivo}/dres/{dreCodigo}/ues/{ueCodigo}/quantidade-alunos")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(QuantidadeCriancaDto), 200)]
        [Permissao(Permissao.DF_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuantidadeCrianca(int anoLetivo, string dreCodigo, string ueCodigo, [FromQuery] string[] turmas, [FromQuery] int[] modalidades, [FromQuery] string[] anoTurma, [FromServices] IObterQuantidadeCriancaUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo, turmas, dreCodigo, ueCodigo, modalidades, anoTurma));
        }
    }
}
