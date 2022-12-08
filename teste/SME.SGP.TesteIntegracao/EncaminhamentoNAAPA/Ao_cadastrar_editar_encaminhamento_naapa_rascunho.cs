using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA
{
    public class Ao_cadastrar_editar_encaminhamento_naapa_rascunho: EncaminhamentoNAAPATesteBase
    {
        
        public Ao_cadastrar_editar_encaminhamento_naapa_rascunho(CollectionFixture collectionFixture) : base(collectionFixture)
        { }
        
        [Fact(DisplayName = "Encaminhamento NAAPA - Cadastrar encaminhamento NAAPA - Concluído")]
        public async Task Ao_cadastrar_encaminhamento_rascunho_concluido()
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
            
            var encaminhamentosNaapaDto = new EncaminhamentoNAAPADto()
            {
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoNAAPA.Rascunho,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno do naapa",
                Secoes = new List<EncaminhamentoNAAPASecaoDto>()
                {
                    new ()
                    {
                        SecaoId = 1,
                        Questoes = new List<EncaminhamentoNAAPASecaoQuestaoDto>()
                        {
                            new ()
                            {
                                QuestaoId = 1,
                                Resposta = dataQueixa.ToString("dd/MM/yyyy"),
                                TipoQuestao = TipoQuestao.Data
                            },
                            new ()
                            {
                                QuestaoId = 2,
                                Resposta = "1",
                                TipoQuestao = TipoQuestao.Combo
                            }
                        }
                    }
                }
            };

            var retorno = await registrarEncaminhamentoNaapaUseCase.Executar(encaminhamentosNaapaDto);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);
            retorno.Auditoria.ShouldNotBeNull();
            retorno.Auditoria.AlteradoEm.HasValue.ShouldBeFalse();
            (retorno.Auditoria.CriadoEm.Year == dataQueixa.Year).ShouldBeTrue();
            
            var encaminhamentoNAAPA = ObterTodos<Dominio.EncaminhamentoNAAPA>();
            encaminhamentoNAAPA.FirstOrDefault()?.Situacao.Equals(SituacaoNAAPA.Rascunho).ShouldBeTrue();
            encaminhamentoNAAPA.Count.ShouldBe(1);
            
            var encaminhamentoNAAPASecao = ObterTodos<EncaminhamentoNAAPASecao>();
            encaminhamentoNAAPASecao.ShouldNotBeNull();
            encaminhamentoNAAPASecao.FirstOrDefault()?.SecaoEncaminhamentoNAAPAId.ShouldBe(1);
            encaminhamentoNAAPASecao.FirstOrDefault()?.Concluido.ShouldBeTrue();
            
            var questaoEncaminhamentoNAAPA = ObterTodos<QuestaoEncaminhamentoNAAPA>();
            questaoEncaminhamentoNAAPA.ShouldNotBeNull();
            questaoEncaminhamentoNAAPA.Count.ShouldBe(2);
            questaoEncaminhamentoNAAPA.Any(a => a.QuestaoId == 1).ShouldBeTrue();
            questaoEncaminhamentoNAAPA.Any(a => a.QuestaoId == 2).ShouldBeTrue();

            var respostaEncaminhamentoNAAPA = ObterTodos<RespostaEncaminhamentoNAAPA>();
            respostaEncaminhamentoNAAPA.ShouldNotBeNull();
            respostaEncaminhamentoNAAPA.Count.ShouldBe(2);
            respostaEncaminhamentoNAAPA.Any(a => a.RespostaId == 1).ShouldBeTrue();
            respostaEncaminhamentoNAAPA.Any(a => a.Texto.Equals(dataQueixa.ToString("dd/MM/yyyy"))).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Encaminhamento NAAPA - Cadastrar encaminhamento NAAPA - Parcialmente concluído")]
        public async Task Ao_cadastrar_encaminhamento_rascunho_parcialmente_concluido()
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
            
            var encaminhamentosNaapaDto = new EncaminhamentoNAAPADto()
            {
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoNAAPA.Rascunho,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno do naapa",
                Secoes = new List<EncaminhamentoNAAPASecaoDto>()
                {
                    new ()
                    {
                        SecaoId = 1,
                        Questoes = new List<EncaminhamentoNAAPASecaoQuestaoDto>()
                        {
                            new ()
                            {
                                QuestaoId = 1,
                                Resposta = dataQueixa.ToString("dd/MM/yyyy"),
                                TipoQuestao = TipoQuestao.Data
                            },
                            new ()
                            {
                                QuestaoId = 2,
                                Resposta = string.Empty,
                                TipoQuestao = TipoQuestao.Combo
                            }
                        }
                    }
                }
            };

            var retorno = await registrarEncaminhamentoNaapaUseCase.Executar(encaminhamentosNaapaDto);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);
            retorno.Auditoria.ShouldNotBeNull();
            retorno.Auditoria.AlteradoEm.HasValue.ShouldBeFalse();
            (retorno.Auditoria.CriadoEm.Year == dataQueixa.Year).ShouldBeTrue();
            
            var encaminhamentoNAAPA = ObterTodos<Dominio.EncaminhamentoNAAPA>();
            encaminhamentoNAAPA.FirstOrDefault()?.Situacao.Equals(SituacaoNAAPA.Rascunho).ShouldBeTrue();
            encaminhamentoNAAPA.Count.ShouldBe(1);
            
            var encaminhamentoNAAPASecao = ObterTodos<EncaminhamentoNAAPASecao>();
            encaminhamentoNAAPASecao.ShouldNotBeNull();
            encaminhamentoNAAPASecao.FirstOrDefault()?.SecaoEncaminhamentoNAAPAId.ShouldBe(1);
            encaminhamentoNAAPASecao.FirstOrDefault()?.Concluido.ShouldBeFalse();
            
            var questaoEncaminhamentoNAAPA = ObterTodos<QuestaoEncaminhamentoNAAPA>();
            questaoEncaminhamentoNAAPA.ShouldNotBeNull();
            questaoEncaminhamentoNAAPA.Count.ShouldBe(2);
            questaoEncaminhamentoNAAPA.Any(a => a.QuestaoId == 1).ShouldBeTrue();
            questaoEncaminhamentoNAAPA.Any(a => a.QuestaoId == 2).ShouldBeTrue();
            
            var respostaEncaminhamentoNAAPA = ObterTodos<RespostaEncaminhamentoNAAPA>();
            respostaEncaminhamentoNAAPA.ShouldNotBeNull();
            respostaEncaminhamentoNAAPA.Count.ShouldBe(2);
            respostaEncaminhamentoNAAPA.Any(a => a.QuestaoEncaminhamentoId == 1).ShouldBeTrue();
            
            respostaEncaminhamentoNAAPA.Where(c => c.QuestaoEncaminhamentoId == 1)
                .Any(a => a.Texto.Equals(dataQueixa.ToString("dd/MM/yyyy"))).ShouldBeTrue(); 
            
            respostaEncaminhamentoNAAPA.Where(c => c.QuestaoEncaminhamentoId == 2)
                .Any(a => a.Texto == null).ShouldBeTrue();            
        }        
        
        [Fact(DisplayName = "Encaminhamento NAAPA - Alterar encaminhamento NAAPA")]
        public async Task Ao_editar_encaminhamento_rascunho()
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

            await GerarDadosEncaminhamentoNAAPA(dataQueixa);

            dataQueixa.AddDays(4);
            
            var encaminhamentosNaapaDto = new EncaminhamentoNAAPADto()
            {
                Id = 1,
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoNAAPA.Rascunho,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno do naapa",
                Secoes = new List<EncaminhamentoNAAPASecaoDto>()
                {
                    new ()
                    {
                        SecaoId = 1,
                        Questoes = new List<EncaminhamentoNAAPASecaoQuestaoDto>()
                        {
                            new ()
                            {
                                RespostaEncaminhamentoId = 1,
                                QuestaoId = 1,
                                Resposta = dataQueixa.ToString("dd/MM/yyyy"),
                                TipoQuestao = TipoQuestao.Data,
                                
                            },
                            new ()
                            {
                                RespostaEncaminhamentoId = 2,
                                QuestaoId = 2,
                                Resposta = "2",
                                TipoQuestao = TipoQuestao.Combo,
                                
                            }
                        }
                    }
                }
            };

            var encaminhamento = ObterTodos<Dominio.EncaminhamentoNAAPA>();
            
            var retorno = await registrarEncaminhamentoNaapaUseCase.Executar(encaminhamentosNaapaDto);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);
            retorno.Auditoria.ShouldNotBeNull();
            retorno.Auditoria.AlteradoEm.HasValue.ShouldBeTrue();
            
            var encaminhamentoNAAPA = ObterTodos<Dominio.EncaminhamentoNAAPA>();
            encaminhamentoNAAPA.FirstOrDefault()?.Situacao.Equals(SituacaoNAAPA.Rascunho).ShouldBeTrue();
            encaminhamentoNAAPA.Count.ShouldBe(1);
            
            var encaminhamentoNAAPASecao = ObterTodos<EncaminhamentoNAAPASecao>();
            encaminhamentoNAAPASecao.ShouldNotBeNull();
            encaminhamentoNAAPASecao.FirstOrDefault().SecaoEncaminhamentoNAAPAId.ShouldBe(1);
            
            var questaoEncaminhamentoNAAPA = ObterTodos<QuestaoEncaminhamentoNAAPA>();
            questaoEncaminhamentoNAAPA.ShouldNotBeNull();
            questaoEncaminhamentoNAAPA.Count.ShouldBe(2);
            questaoEncaminhamentoNAAPA.Any(a=> a.QuestaoId == 1).ShouldBeTrue();
            questaoEncaminhamentoNAAPA.Any(a=> a.QuestaoId == 2).ShouldBeTrue();
            
            var respostaEncaminhamentoNAAPA = ObterTodos<RespostaEncaminhamentoNAAPA>();
            respostaEncaminhamentoNAAPA.ShouldNotBeNull();
            respostaEncaminhamentoNAAPA.Count.ShouldBe(2);
            respostaEncaminhamentoNAAPA.Any(a=> a.RespostaId == 2).ShouldBeTrue();
            respostaEncaminhamentoNAAPA.Any(a=> a.Texto.Equals(dataQueixa.ToString("dd/MM/yyyy"))).ShouldBeTrue();
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Alterar encaminhamento NAAPA em rascunho persistindo respostas (observação obrigatória não preenchida)")]
        public async Task Ao_editar_encaminhamento_em_rascunho_nao_consistir_observacao_obrigatoria_nao_preenchida()
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

            await GerarDadosEncaminhamentoNAAPA(dataQueixa);

            dataQueixa.AddDays(4);

            var encaminhamentosNaapaDto = new EncaminhamentoNAAPADto()
            {
                Id = 1,
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoNAAPA.Rascunho,
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
            var retorno = await registrarEncaminhamentoNaapaUseCase.Executar(encaminhamentosNaapaDto);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);
            retorno.Auditoria.ShouldNotBeNull();
            retorno.Auditoria.AlteradoEm.HasValue.ShouldBeTrue();

            var encaminhamentoNAAPA = ObterTodos<Dominio.EncaminhamentoNAAPA>();
            encaminhamentoNAAPA.FirstOrDefault().Situacao.Equals(SituacaoNAAPA.Rascunho).ShouldBeTrue();
            encaminhamentoNAAPA.Count().ShouldBe(1);

            var encaminhamentoNAAPASecao = ObterTodos<Dominio.EncaminhamentoNAAPASecao>();
            encaminhamentoNAAPASecao.Count().ShouldBe(2);

            var questoesQuestionarioNAAPA = ObterTodos<Dominio.Questao>().Where(questao => questao.NomeComponente == NOME_COMPONENTE_QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS);
            questoesQuestionarioNAAPA.ShouldBeUnique();

            var questoesEncaminhamentoNAAPA = ObterTodos<Dominio.QuestaoEncaminhamentoNAAPA>().Where(questao => questao.QuestaoId == questoesQuestionarioNAAPA.FirstOrDefault().Id);
            questoesEncaminhamentoNAAPA.ShouldBeUnique();

            var respostasEncaminhamentoNAAPASecao2 = ObterTodos<Dominio.RespostaEncaminhamentoNAAPA>();
            respostasEncaminhamentoNAAPASecao2 = respostasEncaminhamentoNAAPASecao2.Where(resposta => resposta.QuestaoEncaminhamentoId == questoesEncaminhamentoNAAPA.FirstOrDefault().Id).ToList();
            respostasEncaminhamentoNAAPASecao2.Count().ShouldBe(2);
            respostasEncaminhamentoNAAPASecao2.Select(resposta => resposta.RespostaId).ShouldContain(ID_OPCAO_RESPOSTA_DOENCA_CRONICA);
            respostasEncaminhamentoNAAPASecao2.Select(resposta => resposta.RespostaId).ShouldContain(ID_OPCAO_RESPOSTA_ADOECE_COM_FREQUENCIA);
        }

        private async Task GerarDadosEncaminhamentoNAAPA(DateTime dataQueixa)
        {
            await CriarEncaminhamentoNAAPA();
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
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 2,
                Texto = "1",
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
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 2,
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
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
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
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}

