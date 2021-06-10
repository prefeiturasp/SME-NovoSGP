using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api
{
    [ApiController]
    [Route("api/v1/itinerancias")]
    [Authorize("Bearer")]
    public class ItineranciaController : ControllerBase
    {

        [HttpGet("objetivos")]
        [ProducesResponseType(typeof(IEnumerable<ItineranciaObjetivosBaseDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterObjetivos([FromServices] IObterObjetivosBaseUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }
       
        [HttpGet("criadores")]
        [ProducesResponseType(typeof(IEnumerable<ItineranciaObjetivosBaseDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterCriadores([FromServices] IObterRfsPorNomesItineranciaUseCase useCase, [FromQuery]string nome)
        {
            return Ok(await useCase.Executar(nome));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ItineranciaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterRegistroItinerancia(long id, [FromServices] IObterItineranciaPorIdUseCase useCase)
        {
            return Ok(await useCase.Executar(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RI_I, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromBody] ItineranciaDto itineranciaDto, [FromServices] ISalvarItineranciaUseCase useCase)
        {
            return Ok(await useCase.Executar(itineranciaDto));
        }

        [HttpPut]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RI_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromServices] IAlterarItineranciaUseCase useCase, [FromBody] ItineranciaDto itineranciaDto)
        {
            return Ok(await useCase.Executar(itineranciaDto));
        }

        [HttpGet("alunos/questoes/{id}")]
        [ProducesResponseType(typeof(IEnumerable<ItineranciaAlunoQuestaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestoesItineranciaAluno(long id, [FromServices] IObterQuestoesItineranciaAlunoUseCase useCase)
        {
            return Ok(await useCase.Executar(id));
        }

        [HttpGet("questoes")]
        [ProducesResponseType(typeof(ItineranciaQuestoesBaseDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestoes([FromServices] IObterQuestoesBaseUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<ItineranciaResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ListaItinerancias([FromQuery] FiltroPesquisaItineranciasDto filtro, [FromServices] IObterItineranciasUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("situacoes")]
        [ProducesResponseType(typeof(List<SituacaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RI_C, Policy = "Bearer")]
        public IActionResult ObterSituacoes()
        {
            var situacoes = Enum.GetValues(typeof(SituacaoItinerancia))
                        .Cast<SituacaoItinerancia>()
                        .Select(d => new { codigo = (int)d, descricao = d.Name() })
                        .ToList();
            return Ok(situacoes);
        }

        [HttpGet("anos-letivos")]
        [ProducesResponseType(typeof(IEnumerable<long>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterAnosLetivos([FromServices] IObterAnosLetivosItineranciaUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }

        [HttpGet("eventos")]
        [ProducesResponseType(typeof(IEnumerable<EventoNomeDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterEventosPorCalendario([FromQuery]long tipoCalendarioId, [FromQuery] long itineranciaId, [FromQuery] string codigoUE, [FromServices] IObterEventosItinerânciaPorTipoCalendarioUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroEventosItineranciaDto(tipoCalendarioId, itineranciaId, codigoUE)));
        }

    }
}