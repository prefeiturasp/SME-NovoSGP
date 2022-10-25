using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAee
{
    public class Ao_cadastrar_encaminhamento_sim : EncaminhamentoAEETesteBase
    {
        private const string RESPOSTA_TEXTO = "RESPOSTA TEXTO";
        private const string RESPOSTA_COMBO_LEITURA = "7";
        private const string RESPOSTA_COMBO_ESCRITA = "8";
        private const string RESPOSTA_COMBO_PAP = "17";
        private const string RESPOSTA_COMBO_SRM = "18";
            
        public Ao_cadastrar_encaminhamento_sim(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Encaminhamento AEE - (Sim) - Professor deve cadastrar encaminhamento com preenchimento correto dos campos obrigatórios")]
        public async Task Deve_cadastrar_com_campos_obrigatorios_preenchidos()
        {
            await CriarDadosBase(ObterFiltroNotas(ObterPerfilProfessor()));

            var encaminhamentoAeeDto = ObterPreenchimentoQuestionarioEncaminhamento();

            var useCase = ObterRegistrarEncaminhamentoAee();
            var retorno  = await useCase.Executar(encaminhamentoAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBeGreaterThan(0);

            var encaminhamentoAeeSecao = ObterTodos<EncaminhamentoAEESecao>();
            encaminhamentoAeeSecao.ShouldNotBeNull();
            encaminhamentoAeeSecao.Count.ShouldBe(2);
            
            var questaoEncaminhamentoAee = ObterTodos<QuestaoEncaminhamentoAEE>();
            questaoEncaminhamentoAee.ShouldNotBeNull();
            questaoEncaminhamentoAee.Count.ShouldBe(17);
            
            var respostaEncaminhamentoAee = ObterTodos<RespostaEncaminhamentoAEE>();
            respostaEncaminhamentoAee.ShouldNotBeNull();
            respostaEncaminhamentoAee.Count.ShouldBe(22);
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - (Sim) - Professor deve cadastrar encaminhamento com preenchimento correto dos campos obrigatórios e omissão de campos não obrigatórios")]
        public async Task Deve_cadastrar_com_campos_obrigatorios_preenchidos_campos_nao_obrigatorios()
        {
            await CriarDadosBase(ObterFiltroNotas(ObterPerfilProfessor()));

            var encaminhamentoAeeDto = ObterPreenchimentoQuestionarioEncaminhamento();
            var questaoObrigatoria21Secao2 = encaminhamentoAeeDto.Secoes.LastOrDefault().Questoes.FirstOrDefault(w => w.QuestaoId == 21);
            var questaoObrigatoria22Secao2 = encaminhamentoAeeDto.Secoes.LastOrDefault().Questoes.FirstOrDefault(w => w.QuestaoId == 22);

            encaminhamentoAeeDto.Secoes.LastOrDefault().Questoes.Remove(questaoObrigatoria21Secao2); 
            encaminhamentoAeeDto.Secoes.LastOrDefault().Questoes.Remove(questaoObrigatoria22Secao2);

            var useCase = ObterRegistrarEncaminhamentoAee();
            var retorno  = await useCase.Executar(encaminhamentoAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBeGreaterThan(0);

            var encaminhamentoAeeSecao = ObterTodos<EncaminhamentoAEESecao>();
            encaminhamentoAeeSecao.ShouldNotBeNull();
            encaminhamentoAeeSecao.Count.ShouldBe(2);
            
            var questaoEncaminhamentoAee = ObterTodos<QuestaoEncaminhamentoAEE>();
            questaoEncaminhamentoAee.ShouldNotBeNull();
            questaoEncaminhamentoAee.Count.ShouldBe(16);
            
            var respostaEncaminhamentoAee = ObterTodos<RespostaEncaminhamentoAEE>();
            respostaEncaminhamentoAee.ShouldNotBeNull();
            respostaEncaminhamentoAee.Count.ShouldBe(20);
        }
        
        [Fact(DisplayName = "Encaminhamento AEE - (Sim) - Professor não deve cadastrar encaminhamento quando não preencher campos obrigatórios")]
        public async Task Nao_deve_cadastrar_com_campos_obrigatorios_nao_preenchidos()
        {
            await CriarDadosBase(ObterFiltroNotas(ObterPerfilProfessor()));

            var encaminhamentoAeeDto = ObterPreenchimentoQuestionarioEncaminhamento();
            var questaoObrigatoria3_1Secao1 = encaminhamentoAeeDto.Secoes.FirstOrDefault().Questoes.FirstOrDefault(w => w.QuestaoId == 4);
            var questaoObrigatoria7Secao2 = encaminhamentoAeeDto.Secoes.LastOrDefault().Questoes.FirstOrDefault(w => w.QuestaoId == 7);
            var questaoObrigatoria9Secao2 = encaminhamentoAeeDto.Secoes.LastOrDefault().Questoes.FirstOrDefault(w => w.QuestaoId == 9);

            encaminhamentoAeeDto.Secoes.FirstOrDefault().Questoes.Remove(questaoObrigatoria3_1Secao1); 
            encaminhamentoAeeDto.Secoes.LastOrDefault().Questoes.Remove(questaoObrigatoria7Secao2); 
            encaminhamentoAeeDto.Secoes.LastOrDefault().Questoes.Remove(questaoObrigatoria9Secao2); 
                
            var useCase = ObterRegistrarEncaminhamentoAee();
            var exceptionAEE = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(encaminhamentoAeeDto));
            Assert.Equal(string.Format(MensagemNegocioEncaminhamentoAee.EXISTEM_QUESTOES_OBRIGATORIAS_NAO_PREENCHIDAS,
                        "Seção: Informações escolares Questões: [2.3], Seção: Descrição do encaminhamento Questões: [2, 3]")
                         , exceptionAEE.Message);
        }

        private EncaminhamentoAeeDto ObterPreenchimentoQuestionarioEncaminhamento()
        {
            return new EncaminhamentoAeeDto()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoAEE.Encaminhado,
                Secoes = new List<EncaminhamentoAEESecaoDto>()
                {
                    new ()
                    {
                        SecaoId = 1,
                        Concluido = false,
                        Questoes = new List<EncaminhamentoAEESecaoQuestaoDto>()
                        {
                            new ()
                            {
                                QuestaoId = 1,
                                Resposta = string.Empty,
                                TipoQuestao = TipoQuestao.InformacoesEscolares
                            },
                            new ()
                            {
                                QuestaoId = 3,
                                Resposta = "1",
                                TipoQuestao = TipoQuestao.Radio,
                            },
                            new ()
                            {
                                QuestaoId = 4, 
                                Resposta = $"{RESPOSTA_TEXTO} - SIM na anterior - Questão 4",
                                TipoQuestao = TipoQuestao.Texto,
                            }
                        }
                    },
                    new ()
                    {
                        SecaoId = 2,
                        Concluido = true,
                        Questoes = new List<EncaminhamentoAEESecaoQuestaoDto>()
                        {
                            new ()
                            {
                                QuestaoId = 6,
                                Resposta = $"{RESPOSTA_TEXTO} - 6",
                                TipoQuestao = TipoQuestao.Texto,
                            },
                            new ()
                            {
                                QuestaoId = 7,
                                Resposta = "4",
                                TipoQuestao = TipoQuestao.Radio,
                            },
                            new ()
                            {
                                QuestaoId = 9,
                                Resposta = $"{RESPOSTA_TEXTO} - 9",
                                TipoQuestao = TipoQuestao.Texto,
                            },
                            new ()
                            {
                                QuestaoId = 10,
                                Resposta = $"{RESPOSTA_TEXTO} - 10",
                                TipoQuestao = TipoQuestao.Texto,
                            },
                            new ()
                            {
                                QuestaoId = 11,
                                Resposta = RESPOSTA_COMBO_LEITURA,
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                            },
                            new ()
                            {
                                QuestaoId = 11,
                                Resposta = RESPOSTA_COMBO_ESCRITA,
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                            },
                            new ()
                            {
                                QuestaoId = 12,//Depende de QuestaoId = 10
                                Resposta = $"{RESPOSTA_TEXTO} - 12",
                                TipoQuestao = TipoQuestao.Texto
                            },
                            new ()
                            {
                                QuestaoId = 14,
                                Resposta = $"{RESPOSTA_TEXTO} - 14",
                                TipoQuestao = TipoQuestao.Texto,
                            },
                            new ()
                            {
                                QuestaoId = 15,
                                Resposta = "11",
                                TipoQuestao = TipoQuestao.Radio,
                            },
                            new ()
                            {
                                QuestaoId = 16, //Depende de cima
                                Resposta = "[{\"id\":1,\"diaSemana\":\"Segunda\",\"atendimentoAtividade\":\"Atendimento/Atividade\",\"localRealizacao\":\"Local de realização\",\"horarioInicio\":\"2022-09-22T10:00:35\",\"horarioTermino\":\"2022-09-22T17:25:35\"}]",
                                TipoQuestao = TipoQuestao.AtendimentoClinico,
                            },
                            new ()
                            {
                                QuestaoId = 17,
                                Resposta = "14",
                                TipoQuestao = TipoQuestao.Radio,
                            },
                            new ()
                            {
                                QuestaoId = 18,//Dependencia de QuestaoId = 17
                                Resposta = RESPOSTA_COMBO_PAP,
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                            },
                            new ()
                            {
                                QuestaoId = 18,//Dependencia de QuestaoId = 15
                                Resposta = RESPOSTA_COMBO_SRM,
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                            },
                            new ()
                            {
                                QuestaoId = 20,
                                Resposta = $"{RESPOSTA_TEXTO} - 20",
                                TipoQuestao = TipoQuestao.Texto,
                            },
                            new ()
                            {
                                QuestaoId = 21,
                                Resposta = "27ECD7CE-D25B-46C2-9B9F-6FB7D6F49E7F",
                                TipoQuestao = TipoQuestao.Upload
                            },
                            new ()
                            {
                                QuestaoId = 21,
                                Resposta = "85FF76A4-1ABF-4D1E-A287-24D2D5301486",
                                TipoQuestao = TipoQuestao.Upload
                            },
                            new ()
                            {
                                QuestaoId = 21,
                                Resposta = "31941CC3-93E1-49CE-8FCA-82FD9EB24E91",
                                TipoQuestao = TipoQuestao.Upload
                            },
                            new ()
                            {
                                QuestaoId = 21,
                                Resposta = "0B8698F1-257D-4CDA-90A5-F98E59A167AF",
                                TipoQuestao = TipoQuestao.Upload
                            },
                            new ()
                            {
                                QuestaoId = 22,
                                Resposta = $"{RESPOSTA_TEXTO} - 22",
                                TipoQuestao = TipoQuestao.Texto
                            }
                        }
                    }
                }
            };
        }

        private FiltroAEEDto ObterFiltroNotas(string perfil)
        {
            return new FiltroAEEDto()
            {
                Perfil = perfil,
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_1,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriarPeriodoEscolar = true,
                AnoTurma = ANO_7,
                ConsiderarAnoAnterior = false,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                CriarQuestionarioAlternativo = true,
                CriarSecaoEncaminhamentoAeeQuestionario = true
            };
        }
    }
}