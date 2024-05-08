using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.RelatorioPAP.Base;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.RelatorioPAP
{
    public class Ao_copiar_relatorio_pap : RelatorioPAPTesteBase
    {
        public Ao_copiar_relatorio_pap(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        [Fact(DisplayName = "Copiar relatório pap para aluno")]
        public async Task Ao_copiar_relatorio_pap_aluno()
        {
            var useCaseCopiar = ServiceProvider.GetService<ICopiarRelatorioPAPUseCase>();
            var dtoCopiar = new CopiarPapDto
            {
                PeriodoRelatorioPAPId = ConstantesTestePAP.PERIODO_RELATORIO_PAP_ID_1,
                CodigoAlunoOrigem = CODIGO_ALUNO_1,
                CodigoTurmaOrigem = TURMA_CODIGO_1,
                CodigoTurma = TURMA_CODIGO_2,
                Estudantes = new List<CopiarPapEstudantesDto>()
                {
                    new() { AlunoCodigo = CODIGO_ALUNO_2, AlunoNome = "Aluno PAP Copiar" }
                },
                Secoes = new List<CopiarSecaoDto>()
                {
                    new()
                    {
                        SecaoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_DIFICULDADES_APRESENTADAS_ID_2,
                        QuestoesIds = new[]
                        {
                            ConstantesTestePAP.QUESTAO_DIFICULDADES_APRESENTADAS_ID_2,
                            ConstantesTestePAP.QUESTAO_OBSERVACAO_ID_3,
                        }
                    },
                    new()
                    {
                        SecaoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_AVANC_APREND_BIMES_ID_3,
                        QuestoesIds = new[]
                        {
                            ConstantesTestePAP.QUESTAO_AVANÇOS_NA_APRENDIZAGEM_DURANTE_O_BIMESTRE_ID_4
                        }
                    },
                    new()
                    {
                        SecaoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_OBS_ID_4,
                        QuestoesIds = new[]
                        {
                            ConstantesTestePAP.QUESTAO_OBSERVACOES_ID_5,
                        }
                    },
                }
            };
            await useCaseCopiar.Executar(dtoCopiar);
            
            var relatorioTurmaAposCopiar = ObterTodos<RelatorioPeriodicoPAPTurma>();
            relatorioTurmaAposCopiar.ShouldNotBeNull();
            relatorioTurmaAposCopiar.Count.ShouldBe(2);
            relatorioTurmaAposCopiar.Any(a => a.TurmaId == TURMA_ID_2 && a.PeriodoRelatorioId == ConstantesTestePAP.PERIODO_RELATORIO_PAP_ID_1).ShouldBeTrue();
            
            var relatorioAlunoAposCopiar = ObterTodos<RelatorioPeriodicoPAPAluno>();
            relatorioAlunoAposCopiar.ShouldNotBeNull();
            relatorioAlunoAposCopiar.Count.ShouldBe(2);
            relatorioAlunoAposCopiar.Any(a => a.CodigoAluno == CODIGO_ALUNO_2 && a.NomeAluno == "Aluno PAP Copiar").ShouldBeTrue();
            
            var relatorioSecaoAposCopiar = ObterTodos<RelatorioPeriodicoPAPSecao>();
            relatorioSecaoAposCopiar.ShouldNotBeNull();
            relatorioSecaoAposCopiar.Count.ShouldBe(7);
            relatorioSecaoAposCopiar.Any(a => a.SecaoRelatorioPeriodicoId == ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_DIFICULDADES_APRESENTADAS_ID_2).ShouldBeTrue();
            relatorioSecaoAposCopiar.Any(a => a.SecaoRelatorioPeriodicoId == ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_AVANC_APREND_BIMES_ID_3).ShouldBeTrue();
            relatorioSecaoAposCopiar.Any(a => a.SecaoRelatorioPeriodicoId == ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_OBS_ID_4).ShouldBeTrue();
            
            var relatorioQuestaoAposCopiar = ObterTodos<RelatorioPeriodicoPAPQuestao>();
            relatorioQuestaoAposCopiar.ShouldNotBeNull();
            relatorioQuestaoAposCopiar.Count.ShouldBe(9);

            var questaoDificuldade = relatorioQuestaoAposCopiar.Find(a => a.QuestaoId == ConstantesTestePAP.QUESTAO_DIFICULDADES_APRESENTADAS_ID_2);
            questaoDificuldade.ShouldNotBeNull();

            var questaoDificuldadeObservacao = relatorioQuestaoAposCopiar.Find(a => a.QuestaoId == ConstantesTestePAP.QUESTAO_OBSERVACAO_ID_3);
            questaoDificuldadeObservacao.ShouldNotBeNull();

            var questaoAvancos = relatorioQuestaoAposCopiar.Find(a => a.QuestaoId == ConstantesTestePAP.QUESTAO_AVANÇOS_NA_APRENDIZAGEM_DURANTE_O_BIMESTRE_ID_4);
            questaoAvancos.ShouldNotBeNull();

            var questaoObsevacao = relatorioQuestaoAposCopiar.Find(a => a.QuestaoId == ConstantesTestePAP.QUESTAO_OBSERVACOES_ID_5);
            questaoObsevacao.ShouldNotBeNull();
            
            var relatorioRespostaAposCopiar = ObterTodos<RelatorioPeriodicoPAPResposta>();
            relatorioRespostaAposCopiar.ShouldNotBeNull();
            relatorioRespostaAposCopiar.Count.ShouldBe(9);

            var respostaDificuldade = relatorioRespostaAposCopiar.Find(r => r.RelatorioPeriodicoQuestaoId == questaoDificuldade.Id);
            respostaDificuldade.ShouldNotBeNull();
            respostaDificuldade.RespostaId.ShouldNotBeNull();
            respostaDificuldade.RespostaId.ShouldBe(long.Parse(ConstantesTestePAP.OPCAO_RESPOSTA_LEITURA_ID));

            var respostaDificuldadeObservacao = relatorioRespostaAposCopiar.Find(r => r.RelatorioPeriodicoQuestaoId == questaoDificuldadeObservacao.Id);
            respostaDificuldadeObservacao.ShouldNotBeNull();
            respostaDificuldadeObservacao.Texto.ShouldBe("Observação dificuldades apresentadas");

            var respostaQuestaoAvancos = relatorioRespostaAposCopiar.Find(r => r.RelatorioPeriodicoQuestaoId == questaoAvancos.Id);
            respostaQuestaoAvancos.ShouldNotBeNull();
            respostaQuestaoAvancos.Texto.ShouldBe("Avanços na aprendizagem durante o bimestre");

            var respostaObsevacao = relatorioRespostaAposCopiar.Find(r => r.RelatorioPeriodicoQuestaoId == questaoObsevacao.Id);
            respostaObsevacao.ShouldNotBeNull();
            respostaObsevacao.Texto.ShouldBe("Observações");
        }

        private async Task<ResultadoRelatorioPAPDto> CriarPapPorAluno()
        {
            await CriarDadosBase(true, true);
            
            await InserirNaBase(new Dominio.Turma()
            {
                Id = 2,
                CodigoTurma = "2",
                DataAtualizacao = DateTimeExtension.HorarioBrasilia(),
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                UeId = 1,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ModalidadeCodigo = Modalidade.Medio
            });
            await InserirNaBase(new Dominio.Turma()
            {
                Id = 1,
                CodigoTurma = "1",
                DataAtualizacao = DateTimeExtension.HorarioBrasilia(),
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                UeId = 1,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ModalidadeCodigo = Modalidade.Medio
            });
            
            var relatorio = new RelatorioPAPDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                AlunoNome = "Pap",
                periodoRelatorioPAPId = ConstantesTestePAP.PERIODO_RELATORIO_PAP_ID_1,
                TurmaId = TURMA_ID_1,
                Secoes = new List<RelatorioPAPSecaoDto>
                {
                    new RelatorioPAPSecaoDto()
                    {
                        SecaoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_DIFICULDADES_APRESENTADAS_ID_2,
                        Respostas = new List<RelatorioPAPRespostaDto>()
                        {
                            new RelatorioPAPRespostaDto()
                            {
                                QuestaoId = ConstantesTestePAP.QUESTAO_DIFICULDADES_APRESENTADAS_ID_2,
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                                Resposta = ConstantesTestePAP.OPCAO_RESPOSTA_LEITURA_ID
                            },
                            new RelatorioPAPRespostaDto()
                            {
                                QuestaoId = ConstantesTestePAP.QUESTAO_OBSERVACAO_ID_3,
                                TipoQuestao = TipoQuestao.EditorTexto,
                                Resposta = "Observação dificuldades apresentadas"
                            }
                        }
                    },
                    new RelatorioPAPSecaoDto()
                    {
                        SecaoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_AVANC_APREND_BIMES_ID_3,
                        Respostas = new List<RelatorioPAPRespostaDto>()
                        {
                            new RelatorioPAPRespostaDto()
                            {
                                QuestaoId = ConstantesTestePAP.QUESTAO_AVANÇOS_NA_APRENDIZAGEM_DURANTE_O_BIMESTRE_ID_4,
                                TipoQuestao = TipoQuestao.EditorTexto,
                                Resposta = "Avanços na aprendizagem durante o bimestre"
                            }
                        }
                    },
                    new RelatorioPAPSecaoDto()
                    {
                        SecaoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_OBS_ID_4,
                        Respostas = new List<RelatorioPAPRespostaDto>()
                        {
                            new RelatorioPAPRespostaDto()
                            {
                                QuestaoId = ConstantesTestePAP.QUESTAO_OBSERVACOES_ID_5,
                                TipoQuestao = TipoQuestao.EditorTexto,
                                Resposta = "Observações"
                            }
                        }
                    }
                }
            };

            var useCase = ServiceProvider.GetService<ISalvarRelatorioPAPUseCase>();

            return await useCase.Executar(relatorio);
        }
    }
}