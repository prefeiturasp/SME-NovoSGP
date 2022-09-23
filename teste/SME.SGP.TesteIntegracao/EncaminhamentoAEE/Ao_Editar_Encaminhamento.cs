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
    public class Ao_Editar_Encaminhamento : EncaminhamentoAEETesteBase
    {
        public Ao_Editar_Encaminhamento(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_editar_encaminhamento_rascunho_professor()
        {
            await CriarDadosBase(ObterFiltro(ObterPerfilProfessor()));

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Rascunho,
                TurmaId = TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new SecaoEncaminhamentoAEE()
            {
                QuestionarioId = 1,
                Nome = "Informações escolares",
                Ordem = 1,
                Etapa = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new EncaminhamentoAEESecao()
            {
                EncaminhamentoAEEId = 1,
                SecaoEncaminhamentoAEEId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 5,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 6,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 7,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE() {
                QuestaoEncaminhamentoId = 1,
                Texto = "Resposta",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 2,
                Texto = string.Empty,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 3,
                Texto = string.Empty,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var dto = new EncaminhamentoAeeDto()
            {
                Id = 1,
                AlunoCodigo = ALUNO_CODIGO_1,
                TurmaId = TURMA_ID_1,
                Secoes = new List<EncaminhamentoAEESecaoDto>()
                {
                    new EncaminhamentoAEESecaoDto()
                    {
                        SecaoId = 1,
                        Questoes = new List<EncaminhamentoAEESecaoQuestaoDto>()
                        {
                            new EncaminhamentoAEESecaoQuestaoDto()
                            {
                                QuestaoId = 5,
                                RespostaEncaminhamentoId = 1,
                                TipoQuestao = TipoQuestao.Texto,
                                Resposta = "Resposta dto 5"
                            },
                            new EncaminhamentoAEESecaoQuestaoDto()
                            {
                                QuestaoId = 6,
                                RespostaEncaminhamentoId = 2,
                                TipoQuestao = TipoQuestao.Texto,
                                Resposta = "Resposta dto 6"
                            },
                            new EncaminhamentoAEESecaoQuestaoDto()
                            {
                                QuestaoId = 7,
                                RespostaEncaminhamentoId = 3,
                                TipoQuestao = TipoQuestao.Texto,
                                Resposta = String.Empty
                            }
                        }
                    }
                }
            };

            var useCase = ObterRegistrarEncaminhamentoAee();
            var dtoResumo = await useCase.Executar(dto);

            var listaDeResposta = ObterTodos<RespostaEncaminhamentoAEE>();
            listaDeResposta.ShouldNotBeNull();
            var resposta1 = listaDeResposta.Find(resposta => resposta.QuestaoEncaminhamentoId == 1);
            resposta1.ShouldNotBeNull();
            resposta1.Texto.ShouldBe("Resposta dto 5");
            var resposta2 = listaDeResposta.Find(resposta => resposta.QuestaoEncaminhamentoId == 2);
            resposta2.ShouldNotBeNull();
            resposta2.Texto.ShouldBe("Resposta dto 6");
            var resposta3 = listaDeResposta.Find(resposta => resposta.QuestaoEncaminhamentoId == 3);
            resposta3.ShouldNotBeNull();
            resposta3.Texto.ShouldBeEmpty();
        }

        [Fact]
        public async Task Ao_editar_encaminhamento_rascunho_e_enviar_professor_cp()
        {
            await CriarDadosBase(ObterFiltro(ObterPerfilCP()));

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Rascunho,
                TurmaId = TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new SecaoEncaminhamentoAEE()
            {
                QuestionarioId = 1,
                Nome = "Informações escolares",
                Ordem = 1,
                Etapa = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new EncaminhamentoAEESecao()
            {
                EncaminhamentoAEEId = 1,
                SecaoEncaminhamentoAEEId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 5,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 6,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 7,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 1,
                Texto = "Resposta",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 2,
                Texto = string.Empty,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 3,
                Texto = string.Empty,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var dto = new EncaminhamentoAeeDto()
            {
                Id = 1,
                AlunoCodigo = ALUNO_CODIGO_1,
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoAEE.Encaminhado,
                Secoes = new List<EncaminhamentoAEESecaoDto>()
                {
                    new EncaminhamentoAEESecaoDto()
                    {
                        SecaoId = 1,
                        Questoes = new List<EncaminhamentoAEESecaoQuestaoDto>()
                        {
                            new EncaminhamentoAEESecaoQuestaoDto()
                            {
                                QuestaoId = 5,
                                RespostaEncaminhamentoId = 1,
                                TipoQuestao = TipoQuestao.Texto,
                                Resposta = "Resposta dto 5"
                            },
                            new EncaminhamentoAEESecaoQuestaoDto()
                            {
                                QuestaoId = 6,
                                RespostaEncaminhamentoId = 2,
                                TipoQuestao = TipoQuestao.Texto,
                                Resposta = "Resposta dto 6"
                            },
                            new EncaminhamentoAEESecaoQuestaoDto()
                            {
                                QuestaoId = 7,
                                RespostaEncaminhamentoId = 3,
                                TipoQuestao = TipoQuestao.Texto,
                                Resposta = "Resposta dto 7"
                            }
                        }
                    }
                }
            };

            var useCase = ObterRegistrarEncaminhamentoAee();
            var dtoResumo = await useCase.Executar(dto);
            var encaminhamento = ObterTodos<Dominio.EncaminhamentoAEE>().FirstOrDefault();
            encaminhamento.ShouldNotBeNull();
            encaminhamento.Situacao.ShouldBe(SituacaoAEE.Encaminhado);
            var listaDeResposta = ObterTodos<RespostaEncaminhamentoAEE>();
            listaDeResposta.ShouldNotBeNull();
            var resposta1 = listaDeResposta.Find(resposta => resposta.QuestaoEncaminhamentoId == 1);
            resposta1.ShouldNotBeNull();
            resposta1.Texto.ShouldBe("Resposta dto 5");
            var resposta2 = listaDeResposta.Find(resposta => resposta.QuestaoEncaminhamentoId == 2);
            resposta2.ShouldNotBeNull();
            resposta2.Texto.ShouldBe("Resposta dto 6");
            var resposta3 = listaDeResposta.Find(resposta => resposta.QuestaoEncaminhamentoId == 3);
            resposta3.ShouldNotBeNull();
            resposta3.Texto.ShouldBe("Resposta dto 7");
        }

        [Fact]
        public async Task Ao_editar_encaminhamento_aguardando_validacao_e_salvar_rascunho_professor_cp()
        {
            await CriarDadosBase(ObterFiltro(ObterPerfilCP()));

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Encaminhado,
                TurmaId = TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new SecaoEncaminhamentoAEE()
            {
                QuestionarioId = 1,
                Nome = "Informações escolares",
                Ordem = 1,
                Etapa = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new EncaminhamentoAEESecao()
            {
                EncaminhamentoAEEId = 1,
                SecaoEncaminhamentoAEEId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 5,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 6,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 7,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 1,
                Texto = "Resposta 1",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 2,
                Texto = "Resposta 2",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 3,
                Texto = "Resposta 3",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var dto = new EncaminhamentoAeeDto()
            {
                Id = 1,
                AlunoCodigo = ALUNO_CODIGO_1,
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoAEE.Rascunho,
                Secoes = new List<EncaminhamentoAEESecaoDto>()
                {
                    new EncaminhamentoAEESecaoDto()
                    {
                        SecaoId = 1,
                        Questoes = new List<EncaminhamentoAEESecaoQuestaoDto>()
                        {
                            new EncaminhamentoAEESecaoQuestaoDto()
                            {
                                QuestaoId = 5,
                                RespostaEncaminhamentoId = 1,
                                TipoQuestao = TipoQuestao.Texto,
                                Resposta = "Resposta dto 5"
                            },
                            new EncaminhamentoAEESecaoQuestaoDto()
                            {
                                QuestaoId = 6,
                                RespostaEncaminhamentoId = 2,
                                TipoQuestao = TipoQuestao.Texto,
                                Resposta = "Resposta dto 6"
                            },
                            new EncaminhamentoAEESecaoQuestaoDto()
                            {
                                QuestaoId = 7,
                                RespostaEncaminhamentoId = 3,
                                TipoQuestao = TipoQuestao.Texto,
                                Resposta = "Resposta dto 7"
                            }
                        }
                    }
                }
            };

            var useCase = ObterRegistrarEncaminhamentoAee();
            var dtoResumo = await useCase.Executar(dto);
            var encaminhamento = ObterTodos<Dominio.EncaminhamentoAEE>().FirstOrDefault();
            encaminhamento.ShouldNotBeNull();
            encaminhamento.Situacao.ShouldBe(SituacaoAEE.Rascunho);
            var listaDeResposta = ObterTodos<RespostaEncaminhamentoAEE>();
            listaDeResposta.ShouldNotBeNull();
            var resposta1 = listaDeResposta.Find(resposta => resposta.QuestaoEncaminhamentoId == 1);
            resposta1.ShouldNotBeNull();
            resposta1.Texto.ShouldBe("Resposta dto 5");
            var resposta2 = listaDeResposta.Find(resposta => resposta.QuestaoEncaminhamentoId == 2);
            resposta2.ShouldNotBeNull();
            resposta2.Texto.ShouldBe("Resposta dto 6");
            var resposta3 = listaDeResposta.Find(resposta => resposta.QuestaoEncaminhamentoId == 3);
            resposta3.ShouldNotBeNull();
            resposta3.Texto.ShouldBe("Resposta dto 7");
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
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222
            };
        }
    }
}
