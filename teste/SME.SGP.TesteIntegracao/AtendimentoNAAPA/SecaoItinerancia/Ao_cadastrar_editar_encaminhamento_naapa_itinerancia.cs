using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.AtendimentoNAAPA;
using SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFake;
using SME.SGP.TesteIntegracao.Informe.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA.SecaoItinerancia
{
    public class Ao_cadastrar_editar_encaminhamento_naapa_itinerancia : AtendimentoNAAPATesteBase
    {
        private const string PROFISSIONAIS_ENVOLVIDOS_02 = "[{\"login\": \"11223344\", \"nome\": \"psicopedagogo 02\"},{\"login\": \"55667788\", \"nome\": \"psicólogo 02\"},{\"login\": \"66778899\", \"nome\": \"coordenador naapa 01\"},{\"login\": \"77889900\", \"nome\": \"assistente social 01\"}]";
        private const string PROFISSIONAIS_ENVOLVIDOS_01 = "[{\"login\": \"11223344\", \"nome\": \"psicopedagogo 01\"},{\"login\": \"55667788\", \"nome\": \"psicólogo 01\"},{\"login\": \"66778899\", \"nome\": \"coordenador naapa 01\"},{\"login\": \"77889900\", \"nome\": \"assistente social 01\"}]";
        
        public Ao_cadastrar_editar_encaminhamento_naapa_itinerancia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaSgpCommand, bool>), typeof(PublicarFilaSgpCommandFakeItinerancia), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Cadastrar encaminhamento NAAPA itinerância (campos obrigatório não preenchidos)")]
        public async Task Ao_cadastrar_encaminhamento_itinerancia_campos_obrigatorios_nao_preenchidos()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.AguardandoAtendimento,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            var useCase = ObterServicoRegistrarEncaminhamentoItinerario();

            var dataQueixa = DateTimeExtension.HorarioBrasilia().Date;
            dataQueixa.AddDays(-10);

            await GerarDadosEncaminhamentoNAAPA(dataQueixa);

            dataQueixa.AddDays(4);

            var dto = new AtendimentoNAAPAItineranciaDto()
            {
                EncaminhamentoId = 1,
                EncaminhamentoNAAPASecao = new AtendimentoNAAPASecaoDto()
                {
                    SecaoId = 3,
                    Questoes = new List<AtendimentoNAAPASecaoQuestaoDto>()
                    {
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                            TipoQuestao = TipoQuestao.Data
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                            TipoQuestao = TipoQuestao.Combo
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                            TipoQuestao = TipoQuestao.ComboMultiplaEscolha
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_DESCRICAO_ATENDIMENTO,
                            TipoQuestao = TipoQuestao.EditorTexto,
                            Resposta = "Descrição do atendimento"
                        }
                    }
                }
            };
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe(string.Format(MensagemNegocioAtendimentoNAAPA.EXISTEM_QUESTOES_OBRIGATORIAS_NAO_PREENCHIDAS,
                                                    "Seção: Apoio e Acompanhamento Questões: [0, 1, 2]"));
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Cadastrar encaminhamento NAAPA itinerância")]
        public async Task Ao_cadastrar_encaminhamento_itinerancia()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.AguardandoAtendimento,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            var useCase = ObterServicoRegistrarEncaminhamentoItinerario();

            var dataQueixa = DateTimeExtension.HorarioBrasilia().Date;
            dataQueixa.AddDays(-10);

            await GerarDadosEncaminhamentoNAAPA(dataQueixa);

            dataQueixa.AddDays(4);

            var dto = new AtendimentoNAAPAItineranciaDto()
            {
                EncaminhamentoId = 1,
                EncaminhamentoNAAPASecao = new AtendimentoNAAPASecaoDto()
                {
                    SecaoId = 3,
                    Questoes = new List<AtendimentoNAAPASecaoQuestaoDto>()
                    {
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                            TipoQuestao = TipoQuestao.Data,
                            Resposta = dataQueixa.ToString("dd/MM/yyyy")
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                            TipoQuestao = TipoQuestao.Combo,
                            Resposta =  ID_ATENDIMENTO_NAO_PRESENCIAL.ToString()
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                            TipoQuestao = TipoQuestao.Combo,
                            Resposta = ID_ACOES_LUDICAS.ToString()
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_DESCRICAO_ATENDIMENTO,
                            TipoQuestao = TipoQuestao.EditorTexto,
                            Resposta = "Descrição do atendimento"
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_PROFISSIONAIS_ENVOLVIDOS,
                            TipoQuestao = TipoQuestao.ProfissionaisEnvolvidos,
                            Resposta = PROFISSIONAIS_ENVOLVIDOS_01
                        }
                    }
                }
            };

            await useCase.Executar(dto);

            var encaminhamentoNAAPASecao = ObterTodos<EncaminhamentoNAAPASecao>();
            encaminhamentoNAAPASecao.Count().ShouldBe(2);

            var questoes = ObterTodos<QuestaoEncaminhamentoNAAPA>();
            questoes.ShouldNotBeNull();
            var respostas = ObterTodos<RespostaEncaminhamentoNAAPA>();
            respostas.ShouldNotBeNull();

            var questao1 = questoes.Find(questao => questao.QuestaoId == ID_QUESTAO_DATA_ATENDIMENTO);
            questao1.ShouldNotBeNull();
            var resposta1 = respostas.Find(resposta => resposta.QuestaoEncaminhamentoId == questao1.Id);
            resposta1.ShouldNotBeNull();
            resposta1.Texto.ShouldBe(dataQueixa.ToString("dd/MM/yyyy"));

            var questao2 = questoes.Find(questao => questao.QuestaoId == ID_QUESTAO_TIPO_ATENDIMENTO);
            questao2.ShouldNotBeNull();
            var resposta2 = respostas.Find(resposta => resposta.QuestaoEncaminhamentoId == questao2.Id);
            resposta2.ShouldNotBeNull();
            resposta2.RespostaId.ShouldBe(ID_ATENDIMENTO_NAO_PRESENCIAL);

            var questao3 = questoes.Find(questao => questao.QuestaoId == ID_QUESTAO_PROCEDIMENTO_TRABALHO);
            questao3.ShouldNotBeNull();
            var resposta3 = respostas.Find(resposta => resposta.QuestaoEncaminhamentoId == questao3.Id);
            resposta3.ShouldNotBeNull();
            resposta3.RespostaId.ShouldBe(ID_ACOES_LUDICAS);

            var questao4 = questoes.Find(questao => questao.QuestaoId == ID_QUESTAO_DESCRICAO_ATENDIMENTO);
            questao4.ShouldNotBeNull();
            var resposta4 = respostas.Find(resposta => resposta.QuestaoEncaminhamentoId == questao4.Id);
            resposta4.ShouldNotBeNull();
            resposta4.Texto.ShouldBe("Descrição do atendimento");

            var questao5 = questoes.Find(questao => questao.QuestaoId == ID_QUESTAO_PROFISSIONAIS_ENVOLVIDOS);
            questao5.ShouldNotBeNull();
            var resposta5 = respostas.Find(resposta => resposta.QuestaoEncaminhamentoId == questao5.Id);
            resposta5.ShouldNotBeNull();
            resposta5.Texto.ShouldBe(PROFISSIONAIS_ENVOLVIDOS_01);
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Editar encaminhamento NAAPA itinerância (campos obrigatório não preenchidos)")]
        public async Task Ao_editar_encaminhamento_itinerancia_campos_obrigatorios_nao_preenchidos()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.AguardandoAtendimento,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            var useCase = ObterServicoRegistrarEncaminhamentoItinerario();

            var dataQueixa = DateTimeExtension.HorarioBrasilia().Date;
            dataQueixa.AddDays(-10);

            await GerarDadosEncaminhamentoNAAPA(dataQueixa);
            await GerarDadosEncaminhamentoNAAPAItinerario(dataQueixa);

            dataQueixa.AddDays(4);

            var dto = new AtendimentoNAAPAItineranciaDto()
            {
                EncaminhamentoId = 1,
                EncaminhamentoNAAPASecaoId = 2,
                EncaminhamentoNAAPASecao = new AtendimentoNAAPASecaoDto()
                {
                    SecaoId = 3,
                    Questoes = new List<AtendimentoNAAPASecaoQuestaoDto>()
                    {
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                            TipoQuestao = TipoQuestao.Data
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                            TipoQuestao = TipoQuestao.Combo
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                            TipoQuestao = TipoQuestao.ComboMultiplaEscolha
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_DESCRICAO_ATENDIMENTO,
                            TipoQuestao = TipoQuestao.EditorTexto,
                            Resposta = "Descrição do atendimento"
                        }
                    }
                }
            };

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe(string.Format(MensagemNegocioAtendimentoNAAPA.EXISTEM_QUESTOES_OBRIGATORIAS_NAO_PREENCHIDAS,
                                                    "Seção: Apoio e Acompanhamento Questões: [0, 1, 2]"));
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Editar encaminhamento NAAPA itinerância")]
        public async Task Ao_editar_encaminhamento_itinerancia()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.AguardandoAtendimento,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            var useCase = ObterServicoRegistrarEncaminhamentoItinerario();

            var dataQueixa = DateTimeExtension.HorarioBrasilia().Date;
            dataQueixa.AddDays(-10);

            await GerarDadosEncaminhamentoNAAPA(dataQueixa);
            await GerarDadosEncaminhamentoNAAPAItinerario(dataQueixa);

            dataQueixa.AddDays(4);

            var dto = new AtendimentoNAAPAItineranciaDto()
            {
                EncaminhamentoId = 1,
                EncaminhamentoNAAPASecaoId = 2,
                EncaminhamentoNAAPASecao = new AtendimentoNAAPASecaoDto()
                {
                    SecaoId = 3,
                    Questoes = new List<AtendimentoNAAPASecaoQuestaoDto>()
                    {
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                            TipoQuestao = TipoQuestao.Data,
                            Resposta = dataQueixa.ToString("dd/MM/yyyy"),
                            RespostaEncaminhamentoId = 3
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                            TipoQuestao = TipoQuestao.Combo,
                            Resposta =  ID_GRUPO_DE_TRABALHO_NAAPA.ToString(),
                            RespostaEncaminhamentoId = 4
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                            TipoQuestao = TipoQuestao.Combo,
                            Resposta = ID_ACOES_LUDICAS.ToString(),
                            RespostaEncaminhamentoId = 5
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_DESCRICAO_ATENDIMENTO,
                            TipoQuestao = TipoQuestao.EditorTexto,
                            Resposta = "Descrição do atendimento alteração",
                            RespostaEncaminhamentoId = 6
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_PROFISSIONAIS_ENVOLVIDOS,
                            TipoQuestao = TipoQuestao.ProfissionaisEnvolvidos,
                            Resposta = PROFISSIONAIS_ENVOLVIDOS_02,
                            RespostaEncaminhamentoId = 7
                        }
                    }
                }
            };

            await useCase.Executar(dto);

            var encaminhamentoNAAPASecao = ObterTodos<EncaminhamentoNAAPASecao>();
            encaminhamentoNAAPASecao.Count().ShouldBe(2);

            var questoes = ObterTodos<QuestaoEncaminhamentoNAAPA>();
            questoes.ShouldNotBeNull();
            var respostas = ObterTodos<RespostaEncaminhamentoNAAPA>();
            respostas.ShouldNotBeNull();

            var questao1 = questoes.Find(questao => questao.QuestaoId == ID_QUESTAO_DATA_ATENDIMENTO);
            questao1.ShouldNotBeNull();
            var resposta1 = respostas.Find(resposta => resposta.QuestaoEncaminhamentoId == questao1.Id);
            resposta1.ShouldNotBeNull();
            resposta1.Texto.ShouldBe(dataQueixa.ToString("dd/MM/yyyy"));

            var questao2 = questoes.Find(questao => questao.QuestaoId == ID_QUESTAO_TIPO_ATENDIMENTO);
            questao2.ShouldNotBeNull();
            var resposta2 = respostas.Find(resposta => resposta.QuestaoEncaminhamentoId == questao2.Id);
            resposta2.ShouldNotBeNull();
            resposta2.RespostaId.ShouldBe(ID_GRUPO_DE_TRABALHO_NAAPA);

            var questao3 = questoes.Find(questao => questao.QuestaoId == ID_QUESTAO_PROCEDIMENTO_TRABALHO);
            questao3.ShouldNotBeNull();
            var resposta3 = respostas.Find(resposta => resposta.QuestaoEncaminhamentoId == questao3.Id);
            resposta3.ShouldNotBeNull();
            resposta3.RespostaId.ShouldBe(ID_ACOES_LUDICAS);

            var questao4 = questoes.Find(questao => questao.QuestaoId == ID_QUESTAO_DESCRICAO_ATENDIMENTO);
            questao4.ShouldNotBeNull();
            var resposta4 = respostas.Find(resposta => resposta.QuestaoEncaminhamentoId == questao4.Id);
            resposta4.ShouldNotBeNull();
            resposta4.Texto.ShouldBe("Descrição do atendimento alteração");

            var questao5 = questoes.Find(questao => questao.QuestaoId == ID_QUESTAO_PROFISSIONAIS_ENVOLVIDOS);
            questao5.ShouldNotBeNull();
            var resposta5 = respostas.Find(resposta => resposta.QuestaoEncaminhamentoId == questao5.Id);
            resposta5.ShouldNotBeNull();
            resposta5.Texto.ShouldBe(PROFISSIONAIS_ENVOLVIDOS_02);
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Editar encaminhamento NAAPA itinerância removendo anexo")]
        public async Task Ao_editar_encaminhamento_itinerancia_removendo_anexo()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.AguardandoAtendimento,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            var useCase = ObterServicoRegistrarEncaminhamentoItinerario();

            var dataQueixa = DateTimeExtension.HorarioBrasilia().Date;
            dataQueixa.AddDays(-10);

            await GerarDadosEncaminhamentoNAAPA(dataQueixa);
            await GerarDadosEncaminhamentoNAAPAItinerarioComAnexos(dataQueixa);

            dataQueixa.AddDays(4);

            //Com DTO abaixo, ao Editar a Itinerancia, excluirá as 2 respostas da questão anexo: 1 por conta de estar sendo enviada no DTO sem resposta e 1 por nem estar sendo enviada no DTO
            var dto = new AtendimentoNAAPAItineranciaDto()
            {
                EncaminhamentoId = 1,
                EncaminhamentoNAAPASecaoId = 2,
                EncaminhamentoNAAPASecao = new AtendimentoNAAPASecaoDto()
                {
                    SecaoId = 3,
                    Questoes = new List<AtendimentoNAAPASecaoQuestaoDto>()
                    {
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                            TipoQuestao = TipoQuestao.Data,
                            Resposta = dataQueixa.ToString("dd/MM/yyyy"),
                            RespostaEncaminhamentoId = 3
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                            TipoQuestao = TipoQuestao.Combo,
                            Resposta =  ID_GRUPO_DE_TRABALHO_NAAPA.ToString(),
                            RespostaEncaminhamentoId = 4
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                            TipoQuestao = TipoQuestao.Combo,
                            Resposta = ID_ACOES_LUDICAS.ToString(),
                            RespostaEncaminhamentoId = 5
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_DESCRICAO_ATENDIMENTO,
                            TipoQuestao = TipoQuestao.EditorTexto,
                            Resposta = "Descrição do atendimento alteração",
                            RespostaEncaminhamentoId = 6
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_ANEXOS_ITINERANCIA,
                            TipoQuestao = TipoQuestao.Upload,
                            Resposta = "",
                            RespostaEncaminhamentoId = 8,
                        }
                    }
                }
            };

            await useCase.Executar(dto);

            var questoes = ObterTodos<QuestaoEncaminhamentoNAAPA>();
            var questaoAnexo = questoes.Find(questao => questao.QuestaoId == ID_QUESTAO_ANEXOS_ITINERANCIA);
            questaoAnexo.ShouldNotBeNull();
            var respostas = ObterTodos<RespostaEncaminhamentoNAAPA>();
            var respostaAnexo = respostas.Where(resposta => resposta.QuestaoEncaminhamentoId == questaoAnexo.Id);
            respostaAnexo.All(r => r.Excluido).ShouldBeTrue();

            var arquivos = ObterTodos<Arquivo>();
            arquivos.Count().ShouldBe(0);
        }

        private async Task<long> GerarDadosEncaminhamentoNAAPA(DateTime dataQueixa)
        {
            var id = await CriarEncaminhamentoNAAPA();
            await CriarEncaminhamentoNAAPASecao();
            await CriarQuestoesEncaminhamentoNAAPA();
            await CriarRespostasEncaminhamentoNAAPA(dataQueixa);
            return id;
        }


        private async Task GerarDadosEncaminhamentoNAAPAItinerarioComAnexos(DateTime dataQueixa)
        {
            await GerarDadosEncaminhamentoNAAPAItinerario(dataQueixa);

            //Anexos - Itinerancia 
            await InserirNaBase(new Arquivo()
            {
                Codigo = Guid.NewGuid(),
                Nome = $"Arquivo 1 Itinerância NAAPA",
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                TipoConteudo = "application/pdf",
                Tipo = TipoArquivo.ItineranciaAtendimentoNAAPA
            });
            await InserirNaBase(new Arquivo()
            {
                Codigo = Guid.NewGuid(),
                Nome = $"Arquivo 2 Itinerância NAAPA",
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                TipoConteudo = "application/pdf",
                Tipo = TipoArquivo.ItineranciaAtendimentoNAAPA
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 7,
                Texto = "",
                ArquivoId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 7,
                Texto = "",
                ArquivoId = 2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task GerarDadosEncaminhamentoNAAPAItinerario(DateTime dataQueixa)
        {
            await CriarEncaminhamentoNAAPASecaoItinerario();
            await CriarQuestoesEncaminhamentoNAAPAItinerario();
            await CriarRespostasEncaminhamentoNAAPAItinerario(dataQueixa);
        }

        private async Task CriarQuestoesEncaminhamentoNAAPA()
        {
            //Id 1
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            //Id 2
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarQuestoesEncaminhamentoNAAPAItinerario()
        {
            //Id 3
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            //Id 4
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            //Id 5
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            //Id 6
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_DESCRICAO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            //Id 7
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_ANEXOS_ITINERANCIA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            //Id 8
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_PROFISSIONAIS_ENVOLVIDOS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarEncaminhamentoNAAPASecao()
        {
            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarEncaminhamentoNAAPASecaoItinerario()
        {
            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task<long> CriarEncaminhamentoNAAPA()
        {
            return await InserirNaBaseAsync(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.AguardandoAtendimento,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarRespostasEncaminhamentoNAAPA(DateTime dataQueixa)
        {
            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 1,
                Texto = dataQueixa.ToString("dd/MM/yyyy"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 2,
                Texto = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarRespostasEncaminhamentoNAAPAItinerario(DateTime dataQueixa)
        {
            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 3,
                Texto = dataQueixa.ToString("dd/MM/yyyy"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 4,
                RespostaId = ID_ATENDIMENTO_NAO_PRESENCIAL,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 5,
                RespostaId = ID_ACOES_LUDICAS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 6,
                Texto = "Descrição do atendimento",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 8,
                Texto = PROFISSIONAIS_ENVOLVIDOS_01,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Cadastrar encaminhamento NAAPA itinerância e Excluir notificações de inatividade de atendimento")]
        public async Task Ao_cadastrar_encaminhamento_itinerancia_excluir_notificacoes()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.AguardandoAtendimento,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);
            var useCase = ObterServicoRegistrarEncaminhamentoItinerario();

            var dataQueixa = DateTimeExtension.HorarioBrasilia().Date;
            dataQueixa.AddDays(-10);

            var encaminhamentoId = await GerarDadosEncaminhamentoNAAPA(dataQueixa);
            await InserirNotificacoesInatividadeAtendimento(encaminhamentoId);

            dataQueixa.AddDays(4);

            var dto = new AtendimentoNAAPAItineranciaDto()
            {
                EncaminhamentoId = 1,
                EncaminhamentoNAAPASecao = new AtendimentoNAAPASecaoDto()
                {
                    SecaoId = 3,
                    Questoes = new List<AtendimentoNAAPASecaoQuestaoDto>()
                    {
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                            TipoQuestao = TipoQuestao.Data,
                            Resposta = dataQueixa.ToString("dd/MM/yyyy")
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                            TipoQuestao = TipoQuestao.Combo,
                            Resposta =  ID_ATENDIMENTO_NAO_PRESENCIAL.ToString()
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                            TipoQuestao = TipoQuestao.Combo,
                            Resposta = ID_ACOES_LUDICAS.ToString()
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_DESCRICAO_ATENDIMENTO,
                            TipoQuestao = TipoQuestao.EditorTexto,
                            Resposta = "Descrição do atendimento"
                        },
                        new ()
                        {
                            QuestaoId = ID_QUESTAO_PROFISSIONAIS_ENVOLVIDOS,
                            TipoQuestao = TipoQuestao.ProfissionaisEnvolvidos,
                            Resposta = PROFISSIONAIS_ENVOLVIDOS_01
                        }
                    }
                }
            };

            await useCase.Executar(dto);
            var notificacoes = ObterTodos<Notificacao>();
            notificacoes.Count().ShouldBe(2);
            notificacoes.All(n => n.Excluida).ShouldBeTrue();
            var notificacoesInatividadeAtendimento = ObterTodos<InatividadeAtendimentoNAAPANotificacao>();
            notificacoesInatividadeAtendimento.Count().ShouldBe(2);
            notificacoesInatividadeAtendimento.All(n => n.Excluido).ShouldBeTrue();
        }

        private async Task InserirNotificacoesInatividadeAtendimento(long encaminhamentoNAAPAId)
        {
            var id = await InserirNaBaseAsync(new Notificacao()
            {
                Codigo = 1,
                Tipo = NotificacaoTipo.InatividadeAtendimentoNAAPA,
                Categoria = NotificacaoCategoria.Aviso,
                Titulo = "Notificação Usuário 01",
                Mensagem = "Notificação Usuário 01",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                UsuarioId = USUARIO_ID_1
            });

            await InserirNaBase(new InatividadeAtendimentoNAAPANotificacao()
            {
                EncaminhamentoNAAPAId = encaminhamentoNAAPAId,
                NotificacaoId = id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            id = await InserirNaBaseAsync (new Notificacao()
            {
                Codigo = 2,
                Tipo = NotificacaoTipo.InatividadeAtendimentoNAAPA,
                Categoria = NotificacaoCategoria.Aviso,
                Titulo = "Notificação Usuário 02",
                Mensagem = "Notificação Usuário 02",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                UsuarioId = USUARIO_ID_1
            });

            await InserirNaBase(new InatividadeAtendimentoNAAPANotificacao()
            {
                EncaminhamentoNAAPAId = encaminhamentoNAAPAId,
                NotificacaoId = id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
