using Bogus.DataSets;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RegistroAcaoBuscaAtiva
{
    public class Ao_cadastrar_editar_registro_acao_busca_ativa : RegistroAcaoBuscaAtivaTesteBase
    {
        
   
        public Ao_cadastrar_editar_registro_acao_busca_ativa(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorCodigoEolQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Registro de Ação - Cadastrar")]
        public async Task Ao_cadastrar_registro_acao()
        {
            var filtro = new FiltroRegistroAcaoDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtro);

            var useCase = ObterUseCaseRegistroAcao();
            var data = DateTimeExtension.HorarioBrasilia().Date;
            var dtoUseCase = ObterRegistroAcaoBuscaAtivaDtoComQuestoesObrigatoriasPreenchidas(data);

            var retorno = await useCase.Executar(dtoUseCase);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);
            retorno.Auditoria.ShouldNotBeNull();
            retorno.Auditoria.AlteradoEm.HasValue.ShouldBeFalse();
            (retorno.Auditoria.CriadoEm.Year == data.Year).ShouldBeTrue();
            
            var registroAcao = ObterTodos<Dominio.RegistroAcaoBuscaAtiva>();
            registroAcao.Count().ShouldBe(1);
            registroAcao.FirstOrDefault().TurmaId.ShouldBe(TURMA_ID_1);
            registroAcao.FirstOrDefault().AlunoCodigo.ShouldBe(ALUNO_CODIGO_1);

            var registroAcaoSecao = ObterTodos<RegistroAcaoBuscaAtivaSecao>();
            registroAcaoSecao.ShouldNotBeNull();
            registroAcaoSecao.FirstOrDefault()?.SecaoRegistroAcaoBuscaAtivaId.ShouldBe(SECAO_REGISTRO_ACAO_ID_1);
            registroAcaoSecao.FirstOrDefault()?.Concluido.ShouldBeTrue();
            
            var questaoregistroAcao = ObterTodos<QuestaoRegistroAcaoBuscaAtiva>();
            questaoregistroAcao.ShouldNotBeNull();
            questaoregistroAcao.Count.ShouldBe(8);
            questaoregistroAcao.Any(a => a.QuestaoId == QUESTAO_1_ID_DATA_REGISTRO_ACAO).ShouldBeTrue();
            questaoregistroAcao.Any(a => a.QuestaoId == QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP).ShouldBeTrue();
            questaoregistroAcao.Any(a => a.QuestaoId == QUESTAO_2_1_ID_CONTATO_COM_RESPONSAVEL).ShouldBeTrue();
            questaoregistroAcao.Any(a => a.QuestaoId == QUESTAO_2_2_ID_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA).ShouldBeTrue();
            questaoregistroAcao.Any(a => a.QuestaoId == QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA).ShouldBeTrue();
            questaoregistroAcao.Any(a => a.QuestaoId == QUESTAO_3_ID_PROCEDIMENTO_REALIZADO).ShouldBeTrue();
            questaoregistroAcao.Any(a => a.QuestaoId == QUESTAO_3_1_ID_QUESTOES_OBS_DURANTE_VISITA).ShouldBeTrue();
            questaoregistroAcao.Any(a => a.QuestaoId == QUESTAO_4_ID_OBS_GERAL).ShouldBeTrue();
            
            var respostaregistroAcao = ObterTodos<RespostaRegistroAcaoBuscaAtiva>();
            respostaregistroAcao.ShouldNotBeNull();
            respostaregistroAcao.Count().ShouldBe(10);
            respostaregistroAcao.Any(a => a.QuestaoRegistroAcaoBuscaAtivaId == questaoregistroAcao.Where(q => q.QuestaoId == QUESTAO_1_ID_DATA_REGISTRO_ACAO).FirstOrDefault().Id
                                          && a.Texto.Equals(data.ToString("dd/MM/yyyy"))).ShouldBeTrue();
            respostaregistroAcao.Any(a => a.QuestaoRegistroAcaoBuscaAtivaId == questaoregistroAcao.Where(q => q.QuestaoId == QUESTAO_4_ID_OBS_GERAL).FirstOrDefault().Id
                                          && a.Texto.Equals("OBS GERAL")).ShouldBeTrue();
            respostaregistroAcao.Where(a => a.QuestaoRegistroAcaoBuscaAtivaId == questaoregistroAcao.Where(q => q.QuestaoId == QUESTAO_3_1_ID_QUESTOES_OBS_DURANTE_VISITA).FirstOrDefault().Id
                                       ).Count().ShouldBe(2);
            respostaregistroAcao.Where(a => a.QuestaoRegistroAcaoBuscaAtivaId == questaoregistroAcao.Where(q => q.QuestaoId == QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA).FirstOrDefault().Id
                                       ).Count().ShouldBe(2);
        }

        [Fact(DisplayName = "Registro de Ação - Consistir questões obrigatórias ao cadastrar")]
        public async Task Ao_cadastrar_registro_acao_consistir_questoes_obrigatorias()
        {
            var filtro = new FiltroRegistroAcaoDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtro);

            var useCase = ObterUseCaseRegistroAcao();
            var dtoUseCase = ObterRegistroAcaoBuscaAtivaDtoSemQuestoesObrigatoriasPreenchidas(DateTimeExtension.HorarioBrasilia().Date);

            var excecao = await Assert.ThrowsAsync<NegocioException>(async () => await useCase.Executar(dtoUseCase));
            excecao.Message.ShouldBe("Existem questões obrigatórias não preenchidas no Registro de Ação: Seção: Registro Ação Busca Ativa Seção 1 Questões: [2.1, 2.2, 2.3, 3.1]");
        }

        [Fact(DisplayName = "Registro de Ação - Não consistir questões obrigatórias complementares ao cadastrar")]
        public async Task Ao_cadastrar_registro_acao_nao_consistir_questoes_obrigatorias_complementares()
        {
            var filtro = new FiltroRegistroAcaoDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtro);

            var useCase = ObterUseCaseRegistroAcao();
            var dtoUseCase = ObterRegistroAcaoBuscaAtivaDtoSemQuestoesObrigatoriasComplementares(DateTimeExtension.HorarioBrasilia().Date);
            var retorno = await useCase.Executar(dtoUseCase);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);
            retorno.Auditoria.ShouldNotBeNull();
            retorno.Auditoria.AlteradoEm.HasValue.ShouldBeFalse();
        }

        [Fact(DisplayName = "Registro de Ação - Editar")]
        public async Task Ao_editar_registro_acao()
        {
            var filtro = new FiltroRegistroAcaoDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };
            var data = DateTimeExtension.HorarioBrasilia().Date;

            await CriarDadosBase(filtro);
            await GerarDadosRegistroAcao_3PrimeirasQuestoes(data);

            var questaoregistroAcao = ObterTodos<QuestaoRegistroAcaoBuscaAtiva>();
            questaoregistroAcao.ShouldNotBeNull();
            questaoregistroAcao.Count().ShouldBe(3);

            var respostaregistroAcao = ObterTodos<RespostaRegistroAcaoBuscaAtiva>();
            respostaregistroAcao.ShouldNotBeNull();
            respostaregistroAcao.Count().ShouldBe(3);

            var useCase = ObterUseCaseRegistroAcao();           
            var dtoUseCase = ObterRegistroAcaoBuscaAtivaDtoComQuestoesObrigatoriasPreenchidas(data);
            PreencherIdsEdicao(dtoUseCase);

            var retorno = await useCase.Executar(dtoUseCase);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);

            var registroAcao = ObterTodos<Dominio.RegistroAcaoBuscaAtiva>();
            registroAcao.Count.ShouldBe(1);

            var registroAcaoSecao = ObterTodos<RegistroAcaoBuscaAtivaSecao>();
            registroAcaoSecao.ShouldNotBeNull();
            registroAcaoSecao.FirstOrDefault()?.SecaoRegistroAcaoBuscaAtivaId.ShouldBe(SECAO_REGISTRO_ACAO_ID_1);
            registroAcaoSecao.FirstOrDefault()?.Concluido.ShouldBeTrue();

            questaoregistroAcao = ObterTodos<QuestaoRegistroAcaoBuscaAtiva>();
            questaoregistroAcao.ShouldNotBeNull();
            questaoregistroAcao.Count.ShouldBe(8);
            questaoregistroAcao.Any(a => a.QuestaoId == QUESTAO_1_ID_DATA_REGISTRO_ACAO).ShouldBeTrue();
            questaoregistroAcao.Any(a => a.QuestaoId == QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP).ShouldBeTrue();
            questaoregistroAcao.Any(a => a.QuestaoId == QUESTAO_2_1_ID_CONTATO_COM_RESPONSAVEL).ShouldBeTrue();
            questaoregistroAcao.Any(a => a.QuestaoId == QUESTAO_2_2_ID_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA).ShouldBeTrue();
            questaoregistroAcao.Any(a => a.QuestaoId == QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA).ShouldBeTrue();
            questaoregistroAcao.Any(a => a.QuestaoId == QUESTAO_3_ID_PROCEDIMENTO_REALIZADO).ShouldBeTrue();
            questaoregistroAcao.Any(a => a.QuestaoId == QUESTAO_3_1_ID_QUESTOES_OBS_DURANTE_VISITA).ShouldBeTrue();
            questaoregistroAcao.Any(a => a.QuestaoId == QUESTAO_4_ID_OBS_GERAL).ShouldBeTrue();

            respostaregistroAcao = ObterTodos<RespostaRegistroAcaoBuscaAtiva>();
            respostaregistroAcao.ShouldNotBeNull();
            respostaregistroAcao.Count().ShouldBe(10);
            respostaregistroAcao.Any(a => a.QuestaoRegistroAcaoBuscaAtivaId == questaoregistroAcao.Where(q => q.QuestaoId == QUESTAO_1_ID_DATA_REGISTRO_ACAO).FirstOrDefault().Id
                                          && a.Texto.Equals(data.ToString("dd/MM/yyyy"))).ShouldBeTrue();
            respostaregistroAcao.Any(a => a.QuestaoRegistroAcaoBuscaAtivaId == questaoregistroAcao.Where(q => q.QuestaoId == QUESTAO_4_ID_OBS_GERAL).FirstOrDefault().Id
                                          && a.Texto.Equals("OBS GERAL")).ShouldBeTrue();
            respostaregistroAcao.Where(a => a.QuestaoRegistroAcaoBuscaAtivaId == questaoregistroAcao.Where(q => q.QuestaoId == QUESTAO_3_1_ID_QUESTOES_OBS_DURANTE_VISITA).FirstOrDefault().Id
                                       ).Count().ShouldBe(2);
            respostaregistroAcao.Where(a => a.QuestaoRegistroAcaoBuscaAtivaId == questaoregistroAcao.Where(q => q.QuestaoId == QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA).FirstOrDefault().Id
                                       ).Count().ShouldBe(2);
        }

        private void PreencherIdsEdicao(RegistroAcaoBuscaAtivaDto dtoUseCase)
        {
            dtoUseCase.Id = 1;
            var secao = dtoUseCase.Secoes.FirstOrDefault();
            var questao = secao.Questoes.Where(q => q.QuestaoId == QUESTAO_1_ID_DATA_REGISTRO_ACAO).FirstOrDefault();
            questao.RespostaRegistroAcaoId = QUESTAO_1_ID_DATA_REGISTRO_ACAO;
            questao = secao.Questoes.Where(q => q.QuestaoId == QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP).FirstOrDefault();
            questao.RespostaRegistroAcaoId = QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP;
            questao = secao.Questoes.Where(q => q.QuestaoId == QUESTAO_3_ID_PROCEDIMENTO_REALIZADO).FirstOrDefault();
            questao.RespostaRegistroAcaoId = QUESTAO_3_ID_PROCEDIMENTO_REALIZADO;
        }

        private RegistroAcaoBuscaAtivaDto ObterRegistroAcaoBuscaAtivaDtoSemQuestoesObrigatoriasPreenchidas(DateTime data)
        {
            var opcoesResposta = ObterTodos<OpcaoResposta>();
            var opcaoRespostaQ2 = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP && q.Nome == "Sim").FirstOrDefault();
            var opcaoRespostaQ3 = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_3_ID_PROCEDIMENTO_REALIZADO && q.Nome == "Visita Domiciliar").FirstOrDefault();

            return new RegistroAcaoBuscaAtivaDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno do naapa",
                Secoes = new List<RegistroAcaoBuscaAtivaSecaoDto>()
                {
                    new ()
                    {
                        SecaoId = SECAO_REGISTRO_ACAO_ID_1,
                        Questoes = new List<RegistroAcaoBuscaAtivaSecaoQuestaoDto>()
                        {
                            new ()
                            {
                                QuestaoId = QUESTAO_1_ID_DATA_REGISTRO_ACAO,
                                Resposta = data.ToString("dd/MM/yyyy"),
                                TipoQuestao = TipoQuestao.Data
                            },
                            new ()
                            {
                                QuestaoId = QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP,
                                Resposta = opcaoRespostaQ2.Id.ToString(),
                                TipoQuestao = TipoQuestao.Combo
                            },
                            new ()
                            {
                                QuestaoId = QUESTAO_3_ID_PROCEDIMENTO_REALIZADO,
                                Resposta = opcaoRespostaQ3.Id.ToString(),
                                TipoQuestao = TipoQuestao.Combo
                            },
                            new ()
                            {
                                QuestaoId = QUESTAO_4_ID_OBS_GERAL,
                                Resposta = "OBS GERAL",
                                TipoQuestao = TipoQuestao.Texto
                            }
                        }
                    }
                }
            };
        }

        private RegistroAcaoBuscaAtivaDto ObterRegistroAcaoBuscaAtivaDtoSemQuestoesObrigatoriasComplementares(DateTime data)
        {
            var opcoesResposta = ObterTodos<OpcaoResposta>();
            var opcaoRespostaQ2 = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP && q.Nome == "Não").FirstOrDefault();
            var opcaoRespostaQ3 = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_3_ID_PROCEDIMENTO_REALIZADO && q.Nome == "Ligação telefonica").FirstOrDefault();

            return new RegistroAcaoBuscaAtivaDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno do naapa",
                Secoes = new List<RegistroAcaoBuscaAtivaSecaoDto>()
                {
                    new ()
                    {
                        SecaoId = SECAO_REGISTRO_ACAO_ID_1,
                        Questoes = new List<RegistroAcaoBuscaAtivaSecaoQuestaoDto>()
                        {
                            new ()
                            {
                                QuestaoId = QUESTAO_1_ID_DATA_REGISTRO_ACAO,
                                Resposta = data.ToString("dd/MM/yyyy"),
                                TipoQuestao = TipoQuestao.Data
                            },
                            new ()
                            {
                                QuestaoId = QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP,
                                Resposta = opcaoRespostaQ2.Id.ToString(),
                                TipoQuestao = TipoQuestao.Combo
                            },
                            new ()
                            {
                                QuestaoId = QUESTAO_3_ID_PROCEDIMENTO_REALIZADO,
                                Resposta = opcaoRespostaQ3.Id.ToString(),
                                TipoQuestao = TipoQuestao.Combo
                            },
                            new ()
                            {
                                QuestaoId = QUESTAO_4_ID_OBS_GERAL,
                                Resposta = "OBS GERAL",
                                TipoQuestao = TipoQuestao.Texto
                            }
                        }
                    }
                }
            };
        }

        private RegistroAcaoBuscaAtivaDto ObterRegistroAcaoBuscaAtivaDtoComQuestoesObrigatoriasPreenchidas(DateTime data)
        {
            var opcoesResposta = ObterTodos<OpcaoResposta>();
            var opcaoRespostaQ2 = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP && q.Nome == "Sim").FirstOrDefault();
            var opcaoRespostaQ21 = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_2_1_ID_CONTATO_COM_RESPONSAVEL && q.Nome == "Sim").FirstOrDefault();
            var opcaoRespostaQ22 = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_2_2_ID_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA && q.Nome == "Sim").FirstOrDefault();
            var opcaoRespostaQ23a = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA && q.Nome == "Ausência por estarem cuidando de irmãos, pais ou avós").FirstOrDefault();
            var opcaoRespostaQ23b = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA && q.Nome == "Há suspeita de ausência por estar realizando trabalho infantil").FirstOrDefault();

            var opcaoRespostaQ3 = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_3_ID_PROCEDIMENTO_REALIZADO && q.Nome == "Visita Domiciliar").FirstOrDefault();
            var opcaoRespostaQ31a = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_3_1_ID_QUESTOES_OBS_DURANTE_VISITA && q.Nome == "Há suspeita de negligência").FirstOrDefault();
            var opcaoRespostaQ31b = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_3_1_ID_QUESTOES_OBS_DURANTE_VISITA && q.Nome == "Há suspeita de violência física").FirstOrDefault();


            return new RegistroAcaoBuscaAtivaDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno do naapa",
                Secoes = new List<RegistroAcaoBuscaAtivaSecaoDto>()
                {
                    new ()
                    {
                        SecaoId = SECAO_REGISTRO_ACAO_ID_1,
                        Questoes = new List<RegistroAcaoBuscaAtivaSecaoQuestaoDto>()
                        {
                            new ()
                            {
                                QuestaoId = QUESTAO_1_ID_DATA_REGISTRO_ACAO,
                                Resposta = data.ToString("dd/MM/yyyy"),
                                TipoQuestao = TipoQuestao.Data
                            },
                            new ()
                            {
                                QuestaoId = QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP,
                                Resposta = opcaoRespostaQ2.Id.ToString(),
                                TipoQuestao = TipoQuestao.Combo
                            },
                            new ()
                            {
                                QuestaoId = QUESTAO_2_1_ID_CONTATO_COM_RESPONSAVEL,
                                Resposta = opcaoRespostaQ21.Id.ToString(),
                                TipoQuestao = TipoQuestao.Combo
                            },
                            new ()
                            {
                                QuestaoId = QUESTAO_2_2_ID_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA,
                                Resposta = opcaoRespostaQ22.Id.ToString(),
                                TipoQuestao = TipoQuestao.Combo
                            },
                            new ()
                            {
                                QuestaoId = QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA,
                                Resposta = opcaoRespostaQ23a.Id.ToString(),
                                TipoQuestao = TipoQuestao.Combo
                            },
                            new ()
                            {
                                QuestaoId = QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA,
                                Resposta = opcaoRespostaQ23b.Id.ToString(),
                                TipoQuestao = TipoQuestao.Combo
                            },
                            new ()
                            {
                                QuestaoId = QUESTAO_3_ID_PROCEDIMENTO_REALIZADO,
                                Resposta = opcaoRespostaQ3.Id.ToString(),
                                TipoQuestao = TipoQuestao.Combo
                            },
                            new ()
                            {
                                QuestaoId = QUESTAO_3_1_ID_QUESTOES_OBS_DURANTE_VISITA,
                                Resposta = opcaoRespostaQ31a.Id.ToString(),
                                TipoQuestao = TipoQuestao.Combo
                            },
                            new ()
                            {
                                QuestaoId = QUESTAO_3_1_ID_QUESTOES_OBS_DURANTE_VISITA,
                                Resposta = opcaoRespostaQ31b.Id.ToString(),
                                TipoQuestao = TipoQuestao.Combo
                            },
                            new ()
                            {
                                QuestaoId = QUESTAO_4_ID_OBS_GERAL,
                                Resposta = "OBS GERAL",
                                TipoQuestao = TipoQuestao.Texto
                            }
                        }
                    }
                }
            };
        }
    }
}

