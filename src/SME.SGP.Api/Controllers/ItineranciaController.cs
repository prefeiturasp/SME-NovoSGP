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
       // [Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterObjetivos([FromServices] IObterObjetivosBaseUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RegistroIndividualDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
      //  [Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterRegistroItinerancia(long id, [FromServices] IObterItineranciaPorIdUseCase useCase)
        {
            return Ok(await useCase.Executar(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
      //  [Permissao(Permissao.REI_I, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromBody] ItineranciaDto itineranciaDto, [FromServices] ISalvarItineranciaUseCase useCase)
        {
            return Ok(await useCase.Executar(itineranciaDto));
        }

        [HttpPut]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
      //  [Permissao(Permissao.RI_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromServices] IAlterarItineranciaUseCase useCase, [FromBody] ItineranciaDto itineranciaDto)
        {
            return Ok(await useCase.Executar(itineranciaDto));
        }

        [HttpGet("alunos/questoes/{id}")]
        [ProducesResponseType(typeof(RegistroIndividualDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
      //  [Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestoesItineranciaAluno(long id, [FromServices] IObterQuestoesItineranciaAlunoUseCase useCase)
        {
            return Ok(await useCase.Executar(id));
        }

        [HttpGet("questoes")]
        [ProducesResponseType(typeof(RegistroIndividualDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
       // [Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestoes([FromServices] IObterQuestoesBaseUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<EncaminhamentoAEEResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ListaItinerancias([FromQuery] FiltroPesquisaItineranciasDto filtro)
        {
            var itineranciaResumoDto = new List<ItineranciaResumoDto>()
            {
                new ItineranciaResumoDto()
                {
                    Id = 1,
                    DataVisita = DateTime.Now,
                    NomeUe = "CEU EMEF BUTANTA",
                    NomeCriancaEstudante = "5 - ALANA FERREIRA DE OLIVEIRA",
                    NomeTurma = "EF - 3C",
                    Situacao = Dominio.Enumerados.SituacaoItinerancia.Digitado
                },
                new ItineranciaResumoDto()
                {
                    Id = 2,
                    DataVisita = DateTime.Now.AddDays(2),
                    NomeUe = "EMEF ALPINO ANDRADA SERPA, TTE",
                    NomeCriancaEstudante = "2 - AUGUSTO VILAS BOA",
                    NomeTurma = "EF - 4B",
                    Situacao = Dominio.Enumerados.SituacaoItinerancia.Digitado
                },
                new ItineranciaResumoDto()
                {
                    Id = 3,
                    DataVisita = DateTime.Now.AddDays(4),
                    NomeUe = "EMEF AMIRIN LIMA, DES",
                    NomeCriancaEstudante = "5 - ALRÉLIO TELIS",
                    NomeTurma = "EF - 5A",
                    Situacao = Dominio.Enumerados.SituacaoItinerancia.Digitado
                },
                new ItineranciaResumoDto()
                {
                    Id = 4,
                    DataVisita = DateTime.Now.AddDays(5),
                    NomeUe = "EMEF AMIRIN LIMA, DES",
                    NomeCriancaEstudante = "2 - JOÃO SILVA",
                    NomeTurma = "EF - 3C",
                    Situacao = Dominio.Enumerados.SituacaoItinerancia.Digitado
                },
                new ItineranciaResumoDto()
                {
                    Id = 5,
                    DataVisita = DateTime.Now.AddDays(6),
                    NomeUe = "EMEF AMIRIN LIMA, DES",
                    NomeCriancaEstudante = "2 - JOÃO SILVA",
                    NomeTurma = "EF - 3C",
                    Situacao = Dominio.Enumerados.SituacaoItinerancia.Digitado
                },
                new ItineranciaResumoDto()
                {
                    Id = 6,
                    DataVisita = DateTime.Now.AddDays(7),
                    NomeUe = "EMEF AMIRIN LIMA, DES",
                    NomeCriancaEstudante = "2 - JOÃO SILVA",
                    NomeTurma = "EF - 3C",
                    Situacao = Dominio.Enumerados.SituacaoItinerancia.Digitado
                },
                new ItineranciaResumoDto()
                {
                    Id = 7,
                    DataVisita = DateTime.Now.AddDays(8),
                    NomeUe = "CEU EMEF BUTANTA",
                    NomeCriancaEstudante = "5 - ALANA FERREIRA DE OLIVEIRA",
                    NomeTurma = "EF - 3C",
                    Situacao = Dominio.Enumerados.SituacaoItinerancia.Digitado
                },
                new ItineranciaResumoDto()
                {
                    Id = 8,
                    DataVisita = DateTime.Now.AddDays(9),
                    NomeUe = "EMEF AMIRIN LIMA, DES",
                    NomeCriancaEstudante = "2 - JOÃO SILVA",
                    NomeTurma = "EF - 3C",
                    Situacao = Dominio.Enumerados.SituacaoItinerancia.Digitado
                },
                new ItineranciaResumoDto()
                {
                    Id = 9,
                    DataVisita = DateTime.Now.AddDays(10),
                    NomeUe = "EMEF AMIRIN LIMA, DES",
                    NomeCriancaEstudante = "2 - JOÃO SILVA",
                    NomeTurma = "EF - 3C",
                    Situacao = Dominio.Enumerados.SituacaoItinerancia.Digitado
                },
                new ItineranciaResumoDto()
                {
                    Id = 10,
                    DataVisita = DateTime.Now.AddDays(11),
                    NomeUe = "EMEF AMIRIN LIMA, DES",
                    NomeCriancaEstudante = "2 - JOÃO SILVA",
                    NomeTurma = "EF - 3C",
                    Situacao = Dominio.Enumerados.SituacaoItinerancia.Digitado
                },
                new ItineranciaResumoDto()
                {
                    Id = 11,
                    DataVisita = DateTime.Now.AddDays(14),
                    NomeUe = "EMEF AMIRIN LIMA, DES",
                    NomeCriancaEstudante = "2 - JOÃO SILVA",
                    NomeTurma = "EF - 3C",
                    Situacao = Dominio.Enumerados.SituacaoItinerancia.Digitado
                },
                new ItineranciaResumoDto()
                {
                    Id = 12,
                    DataVisita = DateTime.Now.AddDays(20),
                    NomeUe = "EMEF AMIRIN LIMA, DES",
                    NomeCriancaEstudante = "2 - JOÃO SILVA",
                    NomeTurma = "EF - 3C",
                    Situacao = Dominio.Enumerados.SituacaoItinerancia.Digitado
                }                
            };
            return Ok(itineranciaResumoDto);
        }
    }
}