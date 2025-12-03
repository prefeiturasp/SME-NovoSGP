using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.EncaminhamentoNAAPA.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA
{
    public class Ao_cadastrar_editar_encaminhamento_naapa_aguardandoatendimento : EncaminhamentoNAAPATesteBase
    {
        
        public Ao_cadastrar_editar_encaminhamento_naapa_aguardandoatendimento(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigoPorIdQuery, string>), typeof(ObterTurmaCodigoPorIdQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorCodigoEolQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));
        }


        [Fact(DisplayName = "Encaminhamento NAAPA - Alterar encaminhamento NAAPA para Aguardando Atendimento (observação obrigatória não preenchida)")]
        public async Task Ao_editar_encaminhamento_para_aguardandoatendimento_consistir_observacao_obrigatoria_nao_preenchida()
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
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            var registrarEncaminhamentoNaapaUseCase = ObterServicoRegistrarEncaminhamento();

            var dataQueixa = DateTimeExtension.HorarioBrasilia().Date;
            dataQueixa.AddDays(-10);
            
            await GerarDadosEncaminhamentoNAAPA(dataQueixa);

            dataQueixa.AddDays(4);
            
            var encaminhamentosNaapaDto = new AtendimentoNAAPADto()
            {
                Id = 1,
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoNAAPA.AguardandoAtendimento,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno do naapa",
                Secoes = new List<EncaminhamentoNAAPASecaoDto>()
                {
                    new ()
                    {
                        SecaoId = 2,
                        Questoes = new List<EncaminhamentoNAAPASecaoQuestaoDto>()
                        {
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                                Resposta = ID_OPCAO_RESPOSTA_DOENCA_CRONICA.ToString(),
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,

                            },
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                                Resposta = ID_OPCAO_RESPOSTA_ADOECE_COM_FREQUENCIA.ToString(),
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,

                            },
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_TIPO_ADOECE_COM_FREQUENCIA,
                                Resposta = ID_OPCAO_RESPOSTA_OUTRAS_QUESTAO_TIPO_ADOECE_COM_FREQUENCIA.ToString(),
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,

                            },
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_TIPO_DOENCA_CRONICA,
                                Resposta = ID_OPCAO_RESPOSTA_OUTRAS_QUESTAO_TIPO_DOENCA_CRONICA.ToString(),
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,

                            }
                        }
                    }
                }
            };
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => registrarEncaminhamentoNaapaUseCase.Executar(encaminhamentosNaapaDto));

            excecao.Message.ShouldBe(String.Format(MensagemNegocioEncaminhamentoNAAPA.EXISTEM_QUESTOES_OBRIGATORIAS_NAO_PREENCHIDAS,
                                                    "Seção: Questões apresentadas Questões: [5]"));
        }


        [Fact(DisplayName = "Encaminhamento NAAPA - Alterar encaminhamento NAAPA para Aguardando Atendimento (observação obrigatória preenchida)")]
        public async Task Ao_editar_encaminhamento_para_aguardandoatendimento_nao_consistir_observacao_obrigatoria_preenchida()
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
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            var registrarEncaminhamentoNaapaUseCase = ObterServicoRegistrarEncaminhamento();

            var dataQueixa = DateTimeExtension.HorarioBrasilia().Date;
            dataQueixa.AddDays(-10);
            await GerarDadosEncaminhamentoNAAPA(dataQueixa);

            dataQueixa.AddDays(4);

            var encaminhamentosNaapaDto = new AtendimentoNAAPADto()
            {
                Id = 1,
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoNAAPA.AguardandoAtendimento,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno do naapa",
                Secoes = new List<EncaminhamentoNAAPASecaoDto>()
                {
                    new ()
                    {
                        SecaoId = 2,
                        Questoes = new List<EncaminhamentoNAAPASecaoQuestaoDto>()
                        {
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                                Resposta = ID_OPCAO_RESPOSTA_DOENCA_CRONICA.ToString(),
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,

                            },
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                                Resposta = ID_OPCAO_RESPOSTA_ADOECE_COM_FREQUENCIA.ToString(),
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,

                            },
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_TIPO_ADOECE_COM_FREQUENCIA,
                                Resposta = ID_OPCAO_RESPOSTA_OUTRAS_QUESTAO_TIPO_ADOECE_COM_FREQUENCIA.ToString(),
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,

                            },
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_TIPO_DOENCA_CRONICA,
                                Resposta = ID_OPCAO_RESPOSTA_OUTRAS_QUESTAO_TIPO_DOENCA_CRONICA.ToString(),
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,

                            },
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_OBS_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                                Resposta = "Observações preenchidas para [Adoece com frequência sem receber cuidados médicos] e [Doença crônica ou em tratamento de longa duração] ",
                                TipoQuestao = TipoQuestao.Texto,

                            }
                        }
                    }
                }
            };
            var retorno = await registrarEncaminhamentoNaapaUseCase.Executar(encaminhamentosNaapaDto);
            
            retorno.ShouldNotBeNull("Nenhum ResultadoEncaminhamentoNAAPADto obtido ao registrar Encaminhamento NAAPA");
            retorno.Id.ShouldBe(1, "Id inválido obtido ao registrar Encaminhamento NAAPA");
            retorno.Auditoria.ShouldNotBeNull("Nenhum registro de Auditoria obtido ao registrar Encaminhamento NAAPA");
            retorno.Auditoria.AlteradoEm.HasValue.ShouldBeTrue("Auditoria obtida não contém data/hora alteração");
            
            var encaminhamentoNAAPA = ObterTodos<Dominio.EncaminhamentoNAAPA>();
            encaminhamentoNAAPA.FirstOrDefault().Situacao.Equals(SituacaoNAAPA.AguardandoAtendimento).ShouldBeTrue("Após registrar Encaminhamento NAAPA o status não foi alterado");
            encaminhamentoNAAPA.Count().ShouldBe(1, "Qdade registros Encaminhamento NAAPA inválidos");
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Alterar encaminhamento NAAPA para Aguardando Atendimento (observação não obrigatória não preenchida)")]
        public async Task Ao_editar_encaminhamento_para_aguardandoatendimento_nao_consistir_observacao_nao_obrigatoria_nao_preenchida()
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
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            var registrarEncaminhamentoNaapaUseCase = ObterServicoRegistrarEncaminhamento();

            var dataQueixa = DateTimeExtension.HorarioBrasilia().Date;
            dataQueixa.AddDays(-10);

            await GerarDadosEncaminhamentoNAAPA(dataQueixa);

            dataQueixa.AddDays(4);

            var encaminhamentosNaapaDto = new AtendimentoNAAPADto()
            {
                Id = 1,
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoNAAPA.AguardandoAtendimento,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno do naapa",
                Secoes = new List<EncaminhamentoNAAPASecaoDto>()
                {
                    new ()
                    {
                        SecaoId = 2,
                        Questoes = new List<EncaminhamentoNAAPASecaoQuestaoDto>()
                        {
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                                Resposta = ID_OPCAO_RESPOSTA_DOENCA_CRONICA.ToString(),
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,

                            },
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                                Resposta = ID_OPCAO_RESPOSTA_ADOECE_COM_FREQUENCIA.ToString(),
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,

                            },
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_TIPO_ADOECE_COM_FREQUENCIA,
                                Resposta = ID_OPCAO_RESPOSTA_ASSADURA_QUESTAO_TIPO_ADOECE_COM_FREQUENCIA.ToString(),
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,

                            },
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_TIPO_DOENCA_CRONICA,
                                Resposta = ID_OPCAO_RESPOSTA_ANEMIA_FALCIFORME_QUESTAO_TIPO_DOENCA_CRONICA.ToString(),
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,

                            }
                        }
                    }
                }
            };
            var retorno = await registrarEncaminhamentoNaapaUseCase.Executar(encaminhamentosNaapaDto);

            retorno.ShouldNotBeNull("Nenhum ResultadoEncaminhamentoNAAPADto obtido ao registrar Encaminhamento NAAPA");
            retorno.Id.ShouldBe(1, "Id inválido obtido ao registrar Encaminhamento NAAPA");
            retorno.Auditoria.ShouldNotBeNull("Nenhum registro de Auditoria obtido ao registrar Encaminhamento NAAPA");
            retorno.Auditoria.AlteradoEm.HasValue.ShouldBeTrue("Auditoria obtida não contém data / hora alteração");

            var encaminhamentoNAAPA = ObterTodos<Dominio.EncaminhamentoNAAPA>();
            encaminhamentoNAAPA.FirstOrDefault().Situacao.Equals(SituacaoNAAPA.AguardandoAtendimento).ShouldBeTrue("Após registrar Encaminhamento NAAPA o status não foi alterado");
            encaminhamentoNAAPA.Count().ShouldBe(1, "Qdade registros Encaminhamento NAAPA inválidos");
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Alterar encaminhamento NAAPA para Aguardando Atendimento persistindo respostas")]
        public async Task Ao_editar_encaminhamento_para_aguardandoatendimento_persistir_novo_status_e_respostas()
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
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            var registrarEncaminhamentoNaapaUseCase = ObterServicoRegistrarEncaminhamento();

            var dataQueixa = DateTimeExtension.HorarioBrasilia().Date;
            dataQueixa.AddDays(-10);
            
            await GerarDadosEncaminhamentoNAAPA(dataQueixa);

            dataQueixa.AddDays(4);

            var encaminhamentosNaapaDto = new AtendimentoNAAPADto()
            {
                Id = 1,
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoNAAPA.AguardandoAtendimento,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno do naapa",
                Secoes = new List<EncaminhamentoNAAPASecaoDto>()
                {
                    new ()
                    {
                        SecaoId = 2,
                        Questoes = new List<EncaminhamentoNAAPASecaoQuestaoDto>()
                        {
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                                Resposta = ID_OPCAO_RESPOSTA_DOENCA_CRONICA.ToString(),
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,

                            },
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                                Resposta = ID_OPCAO_RESPOSTA_ADOECE_COM_FREQUENCIA.ToString(),
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,

                            },
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_TIPO_ADOECE_COM_FREQUENCIA,
                                Resposta = ID_OPCAO_RESPOSTA_OUTRAS_QUESTAO_TIPO_ADOECE_COM_FREQUENCIA.ToString(),
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,

                            },
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_TIPO_DOENCA_CRONICA,
                                Resposta = ID_OPCAO_RESPOSTA_OUTRAS_QUESTAO_TIPO_DOENCA_CRONICA.ToString(),
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,

                            },
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_OBS_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                                Resposta = "Observações preenchidas para [Adoece com frequência sem receber cuidados médicos] e [Doença crônica ou em tratamento de longa duração]",
                                TipoQuestao = TipoQuestao.Texto,

                            }
                        }
                    }
                }
            };
            var retorno = await registrarEncaminhamentoNaapaUseCase.Executar(encaminhamentosNaapaDto);
            retorno.ShouldNotBeNull("Nenhum ResultadoEncaminhamentoNAAPADto obtido ao registrar Encaminhamento NAAPA");
            retorno.Id.ShouldBe(1, "Id inválido obtido ao registrar Encaminhamento NAAPA");
            retorno.Auditoria.ShouldNotBeNull("Nenhum registro de Auditoria obtido ao registrar Encaminhamento NAAPA");
            retorno.Auditoria.AlteradoEm.HasValue.ShouldBeTrue("Auditoria obtida não contém data/hora alteração");

            var encaminhamentoNAAPA = ObterTodos<Dominio.EncaminhamentoNAAPA>();
            encaminhamentoNAAPA.FirstOrDefault().Situacao.Equals(SituacaoNAAPA.AguardandoAtendimento).ShouldBeTrue("Após registrar Encaminhamento NAAPA o status não foi alterado");
            encaminhamentoNAAPA.Count().ShouldBe(1, "Qdade registros Encaminhamento NAAPA inválidos");

            var encaminhamentoNAAPASecao = ObterTodos<Dominio.EncaminhamentoNAAPASecao>();
            encaminhamentoNAAPASecao.Count().ShouldBe(2, "Qdade registros Encaminhamento NAAPA Seção inválidos");

            var questoesQuestionarioNAAPA = ObterTodos<Dominio.Questao>().Where(questao => questao.NomeComponente == NOME_COMPONENTE_QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS);
            questoesQuestionarioNAAPA.ShouldBeUnique("Qdade registros Questão [Agrupamento promoção de cuidados] inválidos");

            var questoesEncaminhamentoNAAPA = ObterTodos<Dominio.QuestaoEncaminhamentoNAAPA>().Where(questao => questao.QuestaoId == questoesQuestionarioNAAPA.FirstOrDefault().Id);
            questoesEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Questão Encaminhamento NAAPA [Agrupamento promoção de cuidados] inválidos");

            var respostasEncaminhamentoNAAPASecao2 = ObterTodos<Dominio.RespostaEncaminhamentoNAAPA>();
            respostasEncaminhamentoNAAPASecao2 = respostasEncaminhamentoNAAPASecao2.Where(resposta => resposta.QuestaoEncaminhamentoId == questoesEncaminhamentoNAAPA.FirstOrDefault().Id).ToList();
            respostasEncaminhamentoNAAPASecao2.Count().ShouldBe(2, "Qdade registros Resposta Encaminhamento NAAPA [Agrupamento promoção de cuidados] inválidos");
            respostasEncaminhamentoNAAPASecao2.Select(resposta => resposta.RespostaId).ShouldContain(ID_OPCAO_RESPOSTA_DOENCA_CRONICA);
            respostasEncaminhamentoNAAPASecao2.Select(resposta => resposta.RespostaId).ShouldContain(ID_OPCAO_RESPOSTA_ADOECE_COM_FREQUENCIA);
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Alterar encaminhamento NAAPA Em Atendimento sem alterar situação")]
        public async Task Ao_editar_encaminhamento_em_atendimento_sem_alterar_situacao()
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
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            var registrarEncaminhamentoNaapaUseCase = ObterServicoRegistrarEncaminhamento();

            var dataQueixa = DateTimeExtension.HorarioBrasilia().Date;
            dataQueixa.AddDays(-10);

            await GerarDadosEncaminhamentoNAAPA(dataQueixa, true);

            dataQueixa.AddDays(4);

            var encaminhamentosNaapaDto = new AtendimentoNAAPADto()
            {
                Id = 1,
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoNAAPA.EmAtendimento,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno do naapa",
                Secoes = new List<EncaminhamentoNAAPASecaoDto>()
                {
                    new ()
                    {
                        SecaoId = 2,
                        Questoes = new List<EncaminhamentoNAAPASecaoQuestaoDto>()
                        {
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                                Resposta = ID_OPCAO_RESPOSTA_DOENCA_CRONICA.ToString(),
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,

                            },
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                                Resposta = ID_OPCAO_RESPOSTA_ADOECE_COM_FREQUENCIA.ToString(),
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,

                            },
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_TIPO_ADOECE_COM_FREQUENCIA,
                                Resposta = ID_OPCAO_RESPOSTA_OUTRAS_QUESTAO_TIPO_ADOECE_COM_FREQUENCIA.ToString(),
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,

                            },
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_TIPO_DOENCA_CRONICA,
                                Resposta = ID_OPCAO_RESPOSTA_OUTRAS_QUESTAO_TIPO_DOENCA_CRONICA.ToString(),
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,

                            },
                            new ()
                            {
                                QuestaoId = ID_QUESTAO_OBS_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                                Resposta = "Observações preenchidas para [Adoece com frequência sem receber cuidados médicos] e [Doença crônica ou em tratamento de longa duração]",
                                TipoQuestao = TipoQuestao.Texto,

                            }
                        }
                    }
                }
            };

            var retorno = await registrarEncaminhamentoNaapaUseCase.Executar(encaminhamentosNaapaDto);
            retorno.ShouldNotBeNull("Nenhum ResultadoEncaminhamentoNAAPADto obtido ao registrar Encaminhamento NAAPA");

            var encaminhamentoNAAPA = ObterTodos<Dominio.EncaminhamentoNAAPA>();
            encaminhamentoNAAPA.Any(en => en.Situacao.Equals(SituacaoNAAPA.EmAtendimento)).ShouldBeTrue();
            
        }

        private async Task GerarDadosEncaminhamentoNAAPA(DateTime dataQueixa, bool situacaoEmAtendimento = false)
        {
            if (!situacaoEmAtendimento)
                await CriarEncaminhamentoNAAPA();
            else
                await CriarEncaminhamentoNAAPAEmAtendimento();

            await CriarEncaminhamentoNAAPASecao();
            await CriarQuestoesEncaminhamentoNAAPA();
            await CriarRespostasEncaminhamentoNAAPA(dataQueixa);
        }

        private async Task CriarRespostasEncaminhamentoNAAPA(DateTime dataQueixa)
        {
            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 1,
                Texto = dataQueixa.ToString("dd/MM/yyyy"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 2,
                Texto = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 3,
                Texto = ID_OPCAO_RESPOSTA_SIM_ESTA_EM_SALA_HOSPITALAR.ToString(),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarQuestoesEncaminhamentoNAAPA()
        {
            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

           await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_ESTA_EM_CLASSE_HOSPITALAR,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarEncaminhamentoNAAPASecao()
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarEncaminhamentoNAAPA()
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.Rascunho,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarEncaminhamentoNAAPAEmAtendimento()
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.EmAtendimento,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}

