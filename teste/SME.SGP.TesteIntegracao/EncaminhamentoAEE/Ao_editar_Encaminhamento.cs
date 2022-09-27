using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.TesteIntegracao.EncaminhamentoAEE;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAee
{
    public class Ao_editar_Encaminhamento : EncaminhamentoAEETesteBase
    {
        private const string RESPOSTA_TEXTO = "RESPOSTA TEXTO";
        private const string RESPOSTA_RADIO_SIM = "1";
        private const string RESPOSTA_RADIO_NAO = "2";
        private const string RESPOSTA_RADIO_NAOSEI = "3";
        private const string RESPOSTA_COMBO_LEITURA = "7";
        private const string RESPOSTA_COMBO_ESCRITA = "8";
        private const string RESPOSTA_COMBO_PAP = "17";
        private const string RESPOSTA_COMBO_SRM = "18";

        public Ao_editar_Encaminhamento(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_editar_encaminhamento_rascunho_professor()
        {
            await CriarDadosBase(ObterFiltro(ObterPerfilProfessor()));
            await InserirEncaminhamentoAEEBase(SituacaoAEE.Rascunho);
            
            var dto = ObterPreenchimentoQuestionarioEncaminhamento();

            var useCase = ObterRegistrarEncaminhamentoAee();
            var dtoResumo = await useCase.Executar(dto);

            var listaDeResposta = ObterTodos<RespostaEncaminhamentoAEE>();
            listaDeResposta.ShouldNotBeNull();
            var resposta1 = listaDeResposta.Find(resposta => resposta.QuestaoEncaminhamentoId == 1);
            resposta1.ShouldNotBeNull();
            resposta1.Texto.ShouldBe(RESPOSTA_RADIO_NAO);
            var resposta2 = listaDeResposta.Find(resposta => resposta.QuestaoEncaminhamentoId == 2);
            resposta2.ShouldNotBeNull();
            resposta2.Texto.ShouldBe($"{RESPOSTA_TEXTO} - 2");
            var resposta3 = listaDeResposta.Find(resposta => resposta.QuestaoEncaminhamentoId == 3);
            resposta3.ShouldNotBeNull();
            resposta3.Texto.ShouldBe(RESPOSTA_RADIO_NAOSEI);
        }

        [Fact]
        public async Task Ao_editar_encaminhamento_rascunho_e_enviar_professor_cp()
        {
            await CriarDadosBase(ObterFiltro(ObterPerfilCP()));
            await InserirEncaminhamentoAEEBase(SituacaoAEE.Rascunho);

            var dto = ObterPreenchimentoQuestionarioEncaminhamento(SituacaoAEE.Encaminhado);

            var useCase = ObterRegistrarEncaminhamentoAee();
            var dtoResumo = await useCase.Executar(dto);
            var encaminhamento = ObterTodos<Dominio.EncaminhamentoAEE>().FirstOrDefault();
            encaminhamento.ShouldNotBeNull();
            encaminhamento.Situacao.ShouldBe(SituacaoAEE.Encaminhado);
            var listaDeResposta = ObterTodos<RespostaEncaminhamentoAEE>();
            listaDeResposta.ShouldNotBeNull();
            var resposta1 = listaDeResposta.Find(resposta => resposta.QuestaoEncaminhamentoId == 1);
            resposta1.ShouldNotBeNull();
            resposta1.Texto.ShouldBe(RESPOSTA_RADIO_NAO);
            var resposta2 = listaDeResposta.Find(resposta => resposta.QuestaoEncaminhamentoId == 2);
            resposta2.ShouldNotBeNull();
            resposta2.Texto.ShouldBe($"{RESPOSTA_TEXTO} - 2");
            var resposta3 = listaDeResposta.Find(resposta => resposta.QuestaoEncaminhamentoId == 3);
            resposta3.ShouldNotBeNull();
            resposta3.Texto.ShouldBe(RESPOSTA_RADIO_NAOSEI);
        }

        [Fact]
        public async Task Ao_editar_encaminhamento_aguardando_validacao_e_salvar_rascunho_professor_cp()
        {
            await CriarDadosBase(ObterFiltro(ObterPerfilCP()));
            await InserirEncaminhamentoAEEBase(SituacaoAEE.Encaminhado);

            var dto = ObterPreenchimentoQuestionarioEncaminhamento();

            var useCase = ObterRegistrarEncaminhamentoAee();
            var dtoResumo = await useCase.Executar(dto);
            var encaminhamento = ObterTodos<Dominio.EncaminhamentoAEE>().FirstOrDefault();
            encaminhamento.ShouldNotBeNull();
            encaminhamento.Situacao.ShouldBe(SituacaoAEE.Rascunho);
            var listaDeResposta = ObterTodos<RespostaEncaminhamentoAEE>();
            listaDeResposta.ShouldNotBeNull();
            var resposta1 = listaDeResposta.Find(resposta => resposta.QuestaoEncaminhamentoId == 1);
            resposta1.ShouldNotBeNull();
            resposta1.Texto.ShouldBe(RESPOSTA_RADIO_NAO);
            var resposta2 = listaDeResposta.Find(resposta => resposta.QuestaoEncaminhamentoId == 2);
            resposta2.ShouldNotBeNull();
            resposta2.Texto.ShouldBe($"{RESPOSTA_TEXTO} - 2");
            var resposta3 = listaDeResposta.Find(resposta => resposta.QuestaoEncaminhamentoId == 3);
            resposta3.ShouldNotBeNull();
            resposta3.Texto.ShouldBe(RESPOSTA_RADIO_NAOSEI);
        }

        private async Task InserirEncaminhamentoAEEBase(SituacaoAEE situacaoAEE)
        {
            //Id 1
            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = ALUNO_CODIGO_1,
                Situacao = situacaoAEE,
                TurmaId = TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //Id 1
            await InserirNaBase(new EncaminhamentoAEESecao()
            {
                EncaminhamentoAEEId = 1,
                SecaoEncaminhamentoAEEId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //Id 2
            await InserirNaBase(new EncaminhamentoAEESecao()
            {
                EncaminhamentoAEEId = 1,
                SecaoEncaminhamentoAEEId = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //Id 1
            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 3,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //Id 2
            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 2,
                QuestaoId = 6,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //Id 3
            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 2,
                QuestaoId = 7,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //Id 1
            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 1,
                Texto = RESPOSTA_RADIO_NAO,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //Id 2
            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 2,
                Texto = $"{RESPOSTA_TEXTO} - 2",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //Id 3
            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 3,
                Texto = RESPOSTA_RADIO_NAOSEI,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private FiltroAEEDto ObterFiltro(string perfil)
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

        private EncaminhamentoAeeDto ObterPreenchimentoQuestionarioEncaminhamento(SituacaoAEE situacao = SituacaoAEE.Rascunho)
        {
            return new EncaminhamentoAeeDto()
            {
                Id = 1,
                AlunoCodigo = ALUNO_CODIGO_1,
                TurmaId = TURMA_ID_1,
                Situacao = situacao,
                Secoes = new List<EncaminhamentoAEESecaoDto>()
                {
                    new ()
                    {
                        SecaoId = 1,
                        Concluido = true,
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
                                Resposta = RESPOSTA_RADIO_NAO,
                                TipoQuestao = TipoQuestao.Radio,
                                RespostaEncaminhamentoId = 1
                            },
                            new ()
                            {
                                QuestaoId = 5,
                                Resposta = $"{RESPOSTA_TEXTO} - Não na anterior - Questão 5",
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
                                Resposta = $"{RESPOSTA_TEXTO} - 2",
                                TipoQuestao = TipoQuestao.Texto,
                                RespostaEncaminhamentoId = 2
                            },
                            new ()
                            {
                                QuestaoId = 7,
                                Resposta = RESPOSTA_RADIO_NAOSEI,
                                TipoQuestao = TipoQuestao.Radio,
                                RespostaEncaminhamentoId = 2
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
                                Resposta = "13",
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
                                Resposta = "15",
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
    }
}
