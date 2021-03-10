﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
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
        public async Task<IActionResult> ObterSituacoes()
        {
            var situacoes = new List<SituacaoDto>() { 
                new SituacaoDto()
                {
                    Codigo = 1,
                    Descricao = "Digitado",
                } 
            };
            return await Task.FromResult(Ok(situacoes));
        }

        [HttpGet("anos-letivos")]
        [ProducesResponseType(typeof(IEnumerable<long>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterAnosLetivos([FromServices] IObterAnosLetivosItineranciaUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }
    }
}