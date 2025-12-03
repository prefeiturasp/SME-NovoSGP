using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using SME.SGP.Infra;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using Shouldly;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA
{
    public class Ao_obter_campos_historico_alteracao_encaminhamento_naapa : EncaminhamentoNAAPATesteBase
    {
        public Ao_obter_campos_historico_alteracao_encaminhamento_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Encaminhamento NAAPA Histórico de alteração - Obter campos alterados com resposta texto ")]
        public async Task Ao_obter_campos_historico_alteracao_campo_resposta_texto()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;

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

            var encaminhamento = new Dominio.EncaminhamentoNAAPA()
            {
                Id = 1,
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.Rascunho,
                MotivoEncerramento = MOTIVO_ENCERRAMENTO,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            };

            var questoes = ObterTodos<Questao>();
            var dataQueixa = new DateTime(dataAtual.Year, 11, 18);
            encaminhamento.Secoes.Add(new EncaminhamentoNAAPASecao()
            {
                Id = 1,
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPA = ObterTodos<SecaoEncaminhamentoNAAPA>().Find(secao => secao.Id == 1),
                Questoes = new List<QuestaoEncaminhamentoNAAPA>()
                {
                    new QuestaoEncaminhamentoNAAPA()
                    {
                        Id = 1,
                        EncaminhamentoNAAPASecaoId = 1,
                        QuestaoId = 1,
                        Questao = questoes.Find(questao => questao.Id == 1),
                        Respostas = new List<RespostaEncaminhamentoNAAPA>()
                        {
                            new RespostaEncaminhamentoNAAPA()
                            {
                                Id = 1,
                                QuestaoEncaminhamentoId= 1,
                                Texto = dataQueixa.ToString("dd/MM/yyyy"),
                                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                            }
                        },
                        CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                    },
                    new QuestaoEncaminhamentoNAAPA()
                    {
                        EncaminhamentoNAAPASecaoId = 1,
                        QuestaoId = 15,
                        Questao = questoes.Find(questao => questao.Id == 15),
                        Respostas = new List<RespostaEncaminhamentoNAAPA>()
                        {
                            new RespostaEncaminhamentoNAAPA()
                            {
                                Id = 2,
                                QuestaoEncaminhamentoId= 2,
                                Texto = "UBS de referência",
                                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                            }
                        },
                        CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                    },
                    new QuestaoEncaminhamentoNAAPA()
                    {
                        EncaminhamentoNAAPASecaoId = 1,
                        QuestaoId = 16,
                        Questao = questoes.Find(questao => questao.Id == 16),
                        Respostas = new List<RespostaEncaminhamentoNAAPA>()
                        {
                            new RespostaEncaminhamentoNAAPA()
                            {
                                Id = 3,
                                QuestaoEncaminhamentoId= 3,
                                Texto = "Teste obsevação",
                                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                            }
                        },
                        CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                    }
                },
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var secaoDto = new AtendimentoNAAPASecaoDto()
            {
                SecaoId = 1,
                Concluido = false,
                Questoes = new List<AtendimentoNAAPASecaoQuestaoDto>()
                {
                    new AtendimentoNAAPASecaoQuestaoDto()
                    {
                        QuestaoId= 1,
                        RespostaEncaminhamentoId=1,
                        Resposta = dataQueixa.AddDays(10).ToString("dd/MM/yyyy"),
                        TipoQuestao = TipoQuestao.Data
                    },
                     new AtendimentoNAAPASecaoQuestaoDto()
                    {
                        QuestaoId= 14,
                        Resposta = "123456",
                        TipoQuestao = TipoQuestao.Numerico
                    },
                    new AtendimentoNAAPASecaoQuestaoDto()
                    {
                        QuestaoId= 15,
                        RespostaEncaminhamentoId=2,
                        Resposta = "UBS de referência",
                        TipoQuestao = TipoQuestao.Texto
                    },
                    new AtendimentoNAAPASecaoQuestaoDto()
                    {
                        QuestaoId= 16,
                        RespostaEncaminhamentoId=3,
                        Resposta = "Obsevação",
                        TipoQuestao = TipoQuestao.EditorTexto
                    }
                }
            };

            var mediator = ServiceProvider.GetService<IMediator>();

            var historico = await mediator.Send(new ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery(secaoDto, encaminhamento.Secoes.FirstOrDefault(), TipoHistoricoAlteracoesEncaminhamentoNAAPA.Alteracao));

            historico.ShouldNotBeNull();

            historico.CamposAlterados.ShouldBe("Data de entrada da queixa | Descrição do encaminhamento");
            historico.EncaminhamentoNAAPAId.ShouldBe(1);
            historico.SecaoEncaminhamentoNAAPAId.ShouldBe(1);
        }

        [Fact(DisplayName = "Encaminhamento NAAPA Histórico de alteração - Obter campos alterados com resposta id ")]
        public async Task Ao_obter_campos_historico_alteracao_campo_resposta_id()
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

            var encaminhamento = new Dominio.EncaminhamentoNAAPA()
            {
                Id = 1,
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.Rascunho,
                MotivoEncerramento = MOTIVO_ENCERRAMENTO,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            };

            var questoes = ObterTodos<Questao>();
            encaminhamento.Secoes.Add(new EncaminhamentoNAAPASecao()
            {
                Id = 1,
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPA = ObterTodos<SecaoEncaminhamentoNAAPA>().Find(secao => secao.Id == 1),
                Questoes = new List<QuestaoEncaminhamentoNAAPA>()
                {
                    new QuestaoEncaminhamentoNAAPA()
                    {
                        Id = 1,
                        EncaminhamentoNAAPASecaoId = 1,
                        QuestaoId = 2,
                        Questao = questoes.Find(questao => questao.Id == 2),
                        Respostas = new List<RespostaEncaminhamentoNAAPA>()
                        {
                            new RespostaEncaminhamentoNAAPA()
                            {
                                Id = 1,
                                QuestaoEncaminhamentoId= 2,
                                RespostaId = 1,
                                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                            }
                        },
                        CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                    },
                    new QuestaoEncaminhamentoNAAPA()
                    {
                        EncaminhamentoNAAPASecaoId = 1,
                        QuestaoId = 3,
                        Questao = questoes.Find(questao => questao.Id == 3),
                        Respostas = new List<RespostaEncaminhamentoNAAPA>()
                        {
                            new RespostaEncaminhamentoNAAPA()
                            {
                                Id = 2,
                                QuestaoEncaminhamentoId= 3,
                                RespostaId = 3,
                                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                            },
                            new RespostaEncaminhamentoNAAPA()
                            {
                                Id = 3,
                                QuestaoEncaminhamentoId= 3,
                                RespostaId = 4,
                                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                            }
                        },
                        CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                    },
                    new QuestaoEncaminhamentoNAAPA()
                    {
                        EncaminhamentoNAAPASecaoId = 1,
                        QuestaoId = 5,
                        Questao = questoes.Find(questao => questao.Id == 5),
                        Respostas = new List<RespostaEncaminhamentoNAAPA>()
                        {
                            new RespostaEncaminhamentoNAAPA()
                            {
                                Id = 4,
                                QuestaoEncaminhamentoId= 5,
                                RespostaId = 9,
                                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                            }
                        },
                        CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                    }
                },
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var secao = encaminhamento.Secoes.FirstOrDefault();

            secao.SecaoEncaminhamentoNAAPA.EncaminhamentoNAAPASecao = secao;

            var secaoDto = new AtendimentoNAAPASecaoDto()
            {
                SecaoId = 1,
                Concluido = false,
                Questoes = new List<AtendimentoNAAPASecaoQuestaoDto>()
                {
                    new AtendimentoNAAPASecaoQuestaoDto()
                    {
                        QuestaoId= 2,
                        RespostaEncaminhamentoId=1,
                        Resposta = "2",
                        TipoQuestao = TipoQuestao.Combo
                    },
                    new AtendimentoNAAPASecaoQuestaoDto()
                    {
                        QuestaoId= 3,
                        RespostaEncaminhamentoId=2,
                        Resposta = "3",
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha
                    },
                    new AtendimentoNAAPASecaoQuestaoDto()
                    {
                        QuestaoId= 3,
                        RespostaEncaminhamentoId=3,
                        Resposta = "4",
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha
                    },
                    new AtendimentoNAAPASecaoQuestaoDto()
                    {
                        QuestaoId= 3,
                        Resposta = "5",
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha
                    },
                    new AtendimentoNAAPASecaoQuestaoDto()
                    {
                        QuestaoId= 4,
                        Resposta = "6",
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha
                    }
                }
            };

            var mediator = ServiceProvider.GetService<IMediator>();

            var historico = await mediator.Send(new ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery(secaoDto, secao, TipoHistoricoAlteracoesEncaminhamentoNAAPA.Alteracao));

            historico.ShouldNotBeNull();

            historico.CamposAlterados.ShouldBe("Prioridade | Questões no agrupamento promoção de cuidados | Selecione um tipo");
            historico.EncaminhamentoNAAPAId.ShouldBe(1);
            historico.SecaoEncaminhamentoNAAPAId.ShouldBe(1);
        }
    }
}
