using Microsoft.AspNetCore.Mvc;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api
{
    [ApiController]
    [Route("api/v1/registros-itinerancias")]
    //[Authorize("Bearer")]
    public class RegistroItineranciaController : ControllerBase
    {

        [HttpGet("obetivos")]
        [ProducesResponseType(typeof(RegistroIndividualDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.REI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterObjetivos()
        {
            var objetivos = new List<RegistroItineranciaObjetivosBaseDto>()
            {
                new RegistroItineranciaObjetivosBaseDto(1, "Mapeamento dos estudantes público da Educação Especial", false, false),
                new RegistroItineranciaObjetivosBaseDto(2, "Reorganização e/ou remanejamento de apoios e serviços", false, false),
                new RegistroItineranciaObjetivosBaseDto(3, "Atendimento de solicitação da U.E", true, false),
                new RegistroItineranciaObjetivosBaseDto(4, "Acompanhamento professor de sala regular", false, true),
                new RegistroItineranciaObjetivosBaseDto(5, "Acompanhamento professor de SRM", false, true),
                new RegistroItineranciaObjetivosBaseDto(6, "Ação Formativa em JEIF", false, true),
                new RegistroItineranciaObjetivosBaseDto(7, "Reunião", false, true),
                new RegistroItineranciaObjetivosBaseDto(8, "Outros", true, false),
            };
            return Ok(objetivos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RegistroIndividualDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.REI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterRegistroItinerancia(long id)
        {
            var itinerancia = new RegistroItineranciaDto()
            {
                DataRetornoVerificacao = new DateTime(),
                DataVisita = new DateTime(),
                Alunos = new List<RegistroItineranciaAlunoDto>()
                {
                    new RegistroItineranciaAlunoDto()
                    {
                        CodigoAluno = "123456",
                        Id = 1,
                        Nome = "João Carlos Almeida",
                        Questoes = new List<RegistroItineranciaAlunoQuestaoDto>()
                        {
                            new RegistroItineranciaAlunoQuestaoDto() {
                                Id=1,
                                QuestaoId = 1,
                                Descricao = "Descritivo do estudante",
                                RegistroItineranciaAlunoId = 1,
                                Resposta = "Teste",
                                Obrigatorio =  true,
                            } ,
                            new RegistroItineranciaAlunoQuestaoDto() {
                                Id=2,
                                QuestaoId = 2,
                                Descricao = "Acompanhamento da situação",
                                RegistroItineranciaAlunoId = 1,
                                Resposta = "Teste",
                                Obrigatorio =  false,
                            } ,
                            new RegistroItineranciaAlunoQuestaoDto() {
                                Id = 3,
                                QuestaoId = 3,
                                Descricao = "Encaminhamentos",
                                RegistroItineranciaAlunoId = 1,
                                Resposta = "Teste",
                                Obrigatorio =  false,
                            } ,
                        }
                    },
                    new RegistroItineranciaAlunoDto()
                    {
                        CodigoAluno = "654321",
                        Id = 1,
                        Nome = "Aline Oliveira"
                        ,
                        Questoes = new List<RegistroItineranciaAlunoQuestaoDto>()
                        {
                            new RegistroItineranciaAlunoQuestaoDto() {
                                Id=1,
                                QuestaoId = 1,
                                Descricao = "Descritivo do estudante",
                                RegistroItineranciaAlunoId = 1,
                                Resposta = "Teste",
                                Obrigatorio =  true,
                            } ,
                            new RegistroItineranciaAlunoQuestaoDto() {
                                Id=2,
                                QuestaoId = 2,
                                Descricao = "Acompanhamento da situação",
                                RegistroItineranciaAlunoId = 1,
                                Resposta = "Teste",
                                Obrigatorio =  false,
                            } ,
                            new RegistroItineranciaAlunoQuestaoDto() {
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
                ObjetivosVisita = new List<RegistroItineranciaObjetivoDto> {
                    new RegistroItineranciaObjetivoDto(1, "Mapeamento dos estudantes público da Educação Especial", false, false, true, "Teste"),
                    new RegistroItineranciaObjetivoDto(2, "Reorganização e/ou remanejamento de apoios e serviços", false, false, true, "teste 1"),
                    new RegistroItineranciaObjetivoDto(3, "Atendimento de solicitação da U.E", true, false, true, "teste 2"),
                },
                Questoes = new List<RegistroItineranciaQuestaoDto>() { 
                    new RegistroItineranciaQuestaoDto() { 
                        Id=1,
                        QuestaoId = 1,
                        Descricao = "Acompanhamento da situação",
                        RegistroItineranciaId = 1,
                        Resposta = "Teste",
                        Obrigatorio = true,
                    } ,
                    new RegistroItineranciaQuestaoDto() {
                        Id = 2,
                        QuestaoId = 2,
                        Descricao = "Encaminhamentos",
                        RegistroItineranciaId = 1,
                        Resposta = "Teste",
                        Obrigatorio = false,
                    } ,
                },
                Ues = new List<RegistroItineranciaUeDto>()
                {
                    new RegistroItineranciaUeDto()
                    {
                        Id = 1,
                        UeId = 1,
                        Descricao = "JT - Máximo de Moura"
                    },
                    new RegistroItineranciaUeDto()
                    {
                        Id = 1,
                        UeId = 1,
                        Descricao = "JT - Jaçanã"
                    }
                }
            };

            return Ok(itinerancia);
        }
    }
}