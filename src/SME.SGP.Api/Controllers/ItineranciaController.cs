using Microsoft.AspNetCore.Mvc;
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
        //[Permissao(Permissao.REI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterObjetivos()
        {
            var objetivos = new List<ItineranciaObjetivosBaseDto>()
            {
                new ItineranciaObjetivosBaseDto(1, "Mapeamento dos estudantes público da Educação Especial", false, false),
                new ItineranciaObjetivosBaseDto(2, "Reorganização e/ou remanejamento de apoios e serviços", false, false),
                new ItineranciaObjetivosBaseDto(3, "Atendimento de solicitação da U.E", true, false),
                new ItineranciaObjetivosBaseDto(4, "Acompanhamento professor de sala regular", false, true),
                new ItineranciaObjetivosBaseDto(5, "Acompanhamento professor de SRM", false, true),
                new ItineranciaObjetivosBaseDto(6, "Ação Formativa em JEIF", false, true),
                new ItineranciaObjetivosBaseDto(7, "Reunião", false, true),
                new ItineranciaObjetivosBaseDto(8, "Outros", true, false),
            };
            return Ok(objetivos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RegistroIndividualDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.REI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterRegistroItinerancia(long id)
        {
            var itinerancia = new ItineranciaDto()
            {
                DataRetornoVerificacao = new DateTime(),
                DataVisita = new DateTime(),
                Alunos = new List<ItineranciaAlunoDto>()
                {
                    new ItineranciaAlunoDto()
                    {
                        CodigoAluno = "123456",
                        Id = 1,
                        Nome = "João Carlos Almeida",
                        Questoes = new List<ItineranciaAlunoQuestaoDto>()
                        {
                            new ItineranciaAlunoQuestaoDto() {
                                Id=1,
                                QuestaoId = 1,
                                Descricao = "Descritivo do estudante",
                                RegistroItineranciaAlunoId = 1,
                                Resposta = "Teste",
                                Obrigatorio =  true,
                            } ,
                            new ItineranciaAlunoQuestaoDto() {
                                Id=2,
                                QuestaoId = 2,
                                Descricao = "Acompanhamento da situação",
                                RegistroItineranciaAlunoId = 1,
                                Resposta = "Teste",
                                Obrigatorio =  false,
                            } ,
                            new ItineranciaAlunoQuestaoDto() {
                                Id = 3,
                                QuestaoId = 3,
                                Descricao = "Encaminhamentos",
                                RegistroItineranciaAlunoId = 1,
                                Resposta = "Teste",
                                Obrigatorio =  false,
                            } ,
                        }
                    },
                    new ItineranciaAlunoDto()
                    {
                        CodigoAluno = "654321",
                        Id = 1,
                        Nome = "Aline Oliveira"
                        ,
                        Questoes = new List<ItineranciaAlunoQuestaoDto>()
                        {
                            new ItineranciaAlunoQuestaoDto() {
                                Id=1,
                                QuestaoId = 1,
                                Descricao = "Descritivo do estudante",
                                RegistroItineranciaAlunoId = 1,
                                Resposta = "Teste",
                                Obrigatorio =  true,
                            } ,
                            new ItineranciaAlunoQuestaoDto() {
                                Id=2,
                                QuestaoId = 2,
                                Descricao = "Acompanhamento da situação",
                                RegistroItineranciaAlunoId = 1,
                                Resposta = "Teste",
                                Obrigatorio =  false,
                            } ,
                            new ItineranciaAlunoQuestaoDto() {
                                Id = 3,
                                QuestaoId = 3,
                                Descricao = "Encaminhamentos",
                                RegistroItineranciaAlunoId = 1,
                                Resposta = "Teste",
                                Obrigatorio =  false,
                            } ,
                        }
                    }
                },
                ObjetivosVisita = new List<ItineranciaObjetivoDto> {
                    new ItineranciaObjetivoDto(1, "Mapeamento dos estudantes público da Educação Especial", false, false, true, "Teste"),
                    new ItineranciaObjetivoDto(2, "Reorganização e/ou remanejamento de apoios e serviços", false, false, true, "teste 1"),
                    new ItineranciaObjetivoDto(3, "Atendimento de solicitação da U.E", true, false, true, "teste 2"),
                },
                Questoes = new List<ItineranciaQuestaoDto>() { 
                    new ItineranciaQuestaoDto() { 
                        Id=1,
                        QuestaoId = 1,
                        Descricao = "Acompanhamento da situação",
                        RegistroItineranciaId = 1,
                        Resposta = "Teste",
                        Obrigatorio = true,
                    } ,
                    new ItineranciaQuestaoDto() {
                        Id = 2,
                        QuestaoId = 2,
                        Descricao = "Encaminhamentos",
                        RegistroItineranciaId = 1,
                        Resposta = "Teste",
                        Obrigatorio = false,
                    } ,
                },
                Ues = new List<ItineranciaUeDto>()
                {
                    new ItineranciaUeDto()
                    {
                        Id = 1,
                        UeId = 1,
                        Descricao = "JT - Máximo de Moura"
                    },
                    new ItineranciaUeDto()
                    {
                        Id = 1,
                        UeId = 1,
                        Descricao = "JT - Jaçanã"
                    }
                }
            };

            return Ok(itinerancia);
        }

        [HttpPost]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.AEE_A, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromBody] ItineranciaDto parametros)
        {
            return Ok(new AuditoriaDto() 
            { Id = 1, 
              CriadoPor = "ALINE LIMA CARVALHO",
              CriadoEm = DateTime.Now,
              CriadoRF = "8240787"
            });
        }

        [HttpGet("questoes-aluno/{idItineranciaAlunoId}")]
        [ProducesResponseType(typeof(RegistroIndividualDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.REI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestoesItineranciaAluno(long itineranciaAlunoId)
        {
            var questoes = new List<ItineranciaAlunoQuestaoDto>()
                        {
                            new ItineranciaAlunoQuestaoDto() {
                                Id=1,
                                QuestaoId = 1,
                                Descricao = "Descritivo do estudante",
                                Resposta = "Teste",
                            } ,
                            new ItineranciaAlunoQuestaoDto() {
                                Id=2,
                                QuestaoId = 2,
                                Descricao = "Acompanhamento da situação",
                                Resposta = "Teste",
                            } ,
                            new ItineranciaAlunoQuestaoDto() {
                                Id = 3,
                                QuestaoId = 3,
                                Descricao = "Encaminhamentos",
                                Resposta = "Teste",
                            } ,
                        };
            return Ok(questoes);
        }
    }
}