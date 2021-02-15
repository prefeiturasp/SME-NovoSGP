using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api
{
    [ApiController]
    [Route("api/v1/itinerancias")]
    //[Authorize("Bearer")]
    public class ItineranciaController : ControllerBase
    {

        [HttpGet("objetivos")]
        [ProducesResponseType(typeof(RegistroIndividualDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterObjetivos([FromServices] IObterObjetivosBaseUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RegistroIndividualDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterRegistroItinerancia(long id, [FromServices] IObterItineranciaPorIdUseCase useCase)
        {
            return Ok(await useCase.Executar(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.REI_I, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromBody] ItineranciaDto itineranciaDto, [FromServices] ISalvarItineranciaUseCase useCase)
        {
            return Ok(await useCase.Executar(itineranciaDto));
        }

        [HttpPut]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.RI_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromServices] IAlterarItineranciaUseCase useCase, [FromBody] ItineranciaDto itineranciaDto)
        {
            return Ok(await useCase.Executar(itineranciaDto));
        }

        [HttpGet("alunos/questoes/{id}")]
        [ProducesResponseType(typeof(RegistroIndividualDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestoesItineranciaAluno(long id, [FromServices] IObterQuestoesItineranciaAlunoUseCase useCase)
        {
            return Ok(await useCase.Executar(id));
        }

        [HttpGet("questoes")]
        [ProducesResponseType(typeof(RegistroIndividualDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestoes([FromServices] IObterQuestoesBaseUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<ItineranciaResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ListaItinerancias([FromQuery] FiltroPesquisaItineranciasDto filtro)
        {

            var listaPaginada = new PaginacaoResultadoDto<ItineranciaResumoDto>()
            {
                TotalPaginas = 10,
                TotalRegistros = 10,
                Items = new List<ItineranciaResumoDto>()
                {
                    new ItineranciaResumoDto()
                {
                    Id = 1,
                    DataVisita = DateTime.Now,
                    UeNome = "CEU EMEF BUTANTA",
                    Nome = "5 - ALANA FERREIRA DE OLIVEIRA",
                    TurmaNome = "EF - 3C"
                },
                new ItineranciaResumoDto()
                {
                    Id = 2,
                    DataVisita = DateTime.Now.AddDays(2),
                    UeNome = "EMEF ALPINO ANDRADA SERPA, TTE",
                    Nome = "2 - AUGUSTO VILAS BOA",
                    TurmaNome = "EF - 4B"
                },
                new ItineranciaResumoDto()
                {
                    Id = 3,
                    DataVisita = DateTime.Now.AddDays(4),
                    UeNome = "EMEF AMIRIN LIMA, DES",
                    Nome = "5 - ALRÉLIO TELIS",
                    TurmaNome = "EF - 5A"
                },
                new ItineranciaResumoDto()
                {
                    Id = 4,
                    DataVisita = DateTime.Now.AddDays(5),
                    UeNome = "EMEF AMIRIN LIMA, DES",
                    Nome = "2 - JOÃO SILVA",
                    TurmaNome = "EF - 3C"
                },
                new ItineranciaResumoDto()
                {
                    Id = 5,
                    DataVisita = DateTime.Now.AddDays(6),
                    UeNome = "EMEF AMIRIN LIMA, DES",
                    Nome = "2 - JOÃO SILVA",
                    TurmaNome = "EF - 3C"
                },
                new ItineranciaResumoDto()
                {
                    Id = 6,
                    DataVisita = DateTime.Now.AddDays(7),
                    UeNome = "EMEF AMIRIN LIMA, DES",
                    Nome = "2 - JOÃO SILVA",
                    TurmaNome = "EF - 3C"
                },
                new ItineranciaResumoDto()
                {
                    Id = 7,
                    DataVisita = DateTime.Now.AddDays(8),
                    UeNome = "CEU EMEF BUTANTA",
                    Nome = "5 - ALANA FERREIRA DE OLIVEIRA",
                    TurmaNome = "EF - 3C"
                },
                new ItineranciaResumoDto()
                {
                    Id = 8,
                    DataVisita = DateTime.Now.AddDays(9),
                    UeNome = "EMEF AMIRIN LIMA, DES",
                    Nome = "2 - JOÃO SILVA",
                    TurmaNome = "EF - 3C"
                },
                new ItineranciaResumoDto()
                {
                    Id = 9,
                    DataVisita = DateTime.Now.AddDays(10),
                    UeNome = "EMEF AMIRIN LIMA, DES",
                    Nome = "2 - JOÃO SILVA",
                    TurmaNome = "EF - 3C"
                },
                new ItineranciaResumoDto()
                {
                    Id = 10,
                    DataVisita = DateTime.Now.AddDays(11),
                    UeNome = "EMEF AMIRIN LIMA, DES",
                    Nome = "2 - JOÃO SILVA",
                    TurmaNome = "EF - 3C"
                },
                new ItineranciaResumoDto()
                {
                    Id = 11,
                    DataVisita = DateTime.Now.AddDays(14),
                    UeNome = "EMEF AMIRIN LIMA, DES",
                    Nome = "2 - JOÃO SILVA",
                    TurmaNome = "EF - 3C"
                },
                new ItineranciaResumoDto()
                {
                    Id = 12,
                    DataVisita = DateTime.Now.AddDays(20),
                    UeNome = "EMEF AMIRIN LIMA, DES",
                    Nome = "2 - JOÃO SILVA",
                    TurmaNome = "EF - 3C"
                }
                }
            };            
            return Ok(listaPaginada);
        }
    }
}