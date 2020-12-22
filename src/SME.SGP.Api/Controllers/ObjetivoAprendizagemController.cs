using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/objetivos-aprendizagem")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class ObjetivoAprendizagemController : ControllerBase
    {
        private readonly IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem;

        public ObjetivoAprendizagemController(IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem)
        {
            this.consultasObjetivoAprendizagem = consultasObjetivoAprendizagem ?? throw new System.ArgumentNullException(nameof(consultasObjetivoAprendizagem));
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<ObjetivoAprendizagemDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_C, Policy = "Bearer")]
        public async Task<IActionResult> Filtrar([FromBody] FiltroObjetivosAprendizagemDto filtroObjetivosAprendizagemDto)
        {
            return Ok(await consultasObjetivoAprendizagem.Filtrar(filtroObjetivosAprendizagemDto));
        }

        [HttpGet]
        [Route("{ano}/{componenteCurricularId}")]
        [ProducesResponseType(typeof(IEnumerable<ObjetivoAprendizagemDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterObjetivosPorAnoEComponenteCurricular([FromServices] IListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase useCase, [FromQuery] bool ensinoEspecial, string ano, long componenteCurricularId)
        {
            var result = await useCase.Executar(ano, componenteCurricularId, ensinoEspecial);
            if (result == null)
                return NoContent();

            return Ok(result);
        }

        [HttpGet]
        [Route("disciplinas/turmas/{turmaId}/componentes/{componenteId}")]
        [ProducesResponseType(typeof(IEnumerable<ComponenteCurricularSimplificadoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> ObterDisciplinasBimestrePlano([FromQuery] DateTime dataAula, long turmaId, long componenteId)
        {
            var disciplinas = await consultasObjetivoAprendizagem.ObterDisciplinasDoBimestrePlanoAnual(dataAula, turmaId, componenteId);

            if (disciplinas.Any())
                return Ok(disciplinas);
            else
                return StatusCode(204);
        }

        [HttpGet]
        [Route("objetivos/turmas/{turmaId}/componentes/{componenteId}/disciplinas/{disciplinaId}")]
        [ProducesResponseType(typeof(IEnumerable<ObjetivosAprendizagemPorComponenteDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> ObterObjetivosPorDisciplina([FromServices] IObterObjetivosPorDisciplinaUseCase UseCase, long turmaId, long componenteId, long disciplinaId, [FromQuery] DateTime dataReferencia, [FromQuery] bool regencia)
        {
            var objetivos = await UseCase.Executar(dataReferencia, turmaId, componenteId, disciplinaId, regencia);

            if (objetivos.Any())
                return Ok(objetivos);
            else
                return StatusCode(204);
        }

        [HttpPost]
        [Route("sincronizar-jurema")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> SincronizarObjetivos([FromServices] IServicoObjetivosAprendizagem servicoObjetivosAprendizagem)
        {
            await servicoObjetivosAprendizagem.SincronizarObjetivosComJurema();
            return Ok();
        }
    }
}