using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
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
        public async Task<IActionResult> Filtrar([FromBody]FiltroObjetivosAprendizagemDto filtroObjetivosAprendizagemDto)
        {
            return Ok(await consultasObjetivoAprendizagem.Filtrar(filtroObjetivosAprendizagemDto));
        }

        [HttpGet]
        [Route("disciplinas/turmas/{turmaId}/componentes/{componenteId}")]
        [ProducesResponseType(typeof(IEnumerable<ComponenteCurricularSimplificadoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> ObterDisciplinasBimestrePlano([FromQuery] DateTime dataAula, long turmaId, long componenteId)
        {
            var anoLetivo = dataAula.Year;
            int bimestre = (dataAula.Month + 2) / 3;

            var disciplinas = await consultasObjetivoAprendizagem.ObterDisciplinasDoBimestrePlanoAnual(anoLetivo, bimestre, turmaId, componenteId);

            if (disciplinas.Any())
                return Ok(disciplinas);
            else
                return StatusCode(204);
        }

        [HttpGet]
        [Route("objetivos/turmas/{turmaId}/componentes/{componenteId}/disciplinas/{disciplinaId}")]
        [ProducesResponseType(typeof(IEnumerable<ComponenteCurricularSimplificadoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> ObterObjetivosPorDisciplina([FromQuery] DateTime dataAula, long turmaId, long componenteId, long disciplinaId, bool regencia)
        {
            var anoLetivo = dataAula.Year;
            int bimestre = (dataAula.Month + 2) / 3;

            var objetivos = await consultasObjetivoAprendizagem.ObterObjetivosPlanoDisciplina(anoLetivo, bimestre, turmaId, componenteId, disciplinaId, regencia);

            if (objetivos.Any())
                return Ok(objetivos);
            else
                return StatusCode(204);
        }
    }
}