using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.RelatorioPAP.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RelatorioPAP
{
    public class Ao_alterar_relatorio_pap : RelatorioPAPTesteBase
    {
        public Ao_alterar_relatorio_pap(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Alterar relatório pap para aluno")]
        public async Task Ao_alterar_relatorio_pap_aluno()
        {
            await CriarDadosBase(true, true);

            await InserirNaBase(new RelatorioPeriodicoPAPTurma()
            {
                PeriodoRelatorioId = ConstantesTestePAP.PERIODO_RELATORIO_PAP_ID_1,
                TurmaId = TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPAluno()
            {
                NomeAluno = "Pap",
                CodigoAluno = CODIGO_ALUNO_1,
                RelatorioPeriodicoTurmaId = ConstantesTestePAP.PAP_TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPSecao()
            {
                RelatorioPeriodicoAlunoId = ConstantesTestePAP.PAP_ALUNO_ID_1,
                SecaoRelatorioPeriodicoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_DIFICULDADES_APRESENTADAS_ID_2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPSecao()
            {
                RelatorioPeriodicoAlunoId = ConstantesTestePAP.PAP_ALUNO_ID_1,
                SecaoRelatorioPeriodicoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_AVANC_APREND_BIMES_ID_3,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPSecao()
            {
                RelatorioPeriodicoAlunoId = ConstantesTestePAP.PAP_ALUNO_ID_1,
                SecaoRelatorioPeriodicoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_OBS_ID_4,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPQuestao()
            {
                RelatorioPeriodiocoSecaoId = ConstantesTestePAP.PAP_SECAO_ID_1,
                QuestaoId = ConstantesTestePAP.QUESTAO_DIFICULDADES_APRESENTADAS_ID_2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPQuestao()
            {
                RelatorioPeriodiocoSecaoId = ConstantesTestePAP.PAP_SECAO_ID_1,
                QuestaoId = ConstantesTestePAP.QUESTAO_OBSERVACAO_ID_3,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPQuestao()
            {
                RelatorioPeriodiocoSecaoId = ConstantesTestePAP.PAP_SECAO_ID_2,
                QuestaoId = ConstantesTestePAP.QUESTAO_AVANÇOS_NA_APRENDIZAGEM_DURANTE_O_BIMESTRE_ID_4,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPQuestao()
            {
                RelatorioPeriodiocoSecaoId = ConstantesTestePAP.PAP_SECAO_ID_3,
                QuestaoId = ConstantesTestePAP.QUESTAO_OBSERVACOES_ID_5,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPResposta()
            {
                RelatorioPeriodicoQuestaoId = ConstantesTestePAP.PAP_QUESTAO_ID_1,
                RespostaId = ConstantesTestePAP.OPCAO_RESPOSTA_ESCRITA_ID,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPResposta()
            {
                RelatorioPeriodicoQuestaoId = ConstantesTestePAP.PAP_QUESTAO_ID_2,
                Texto = "Observação dificuldades apresentadas",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPResposta()
            {
                RelatorioPeriodicoQuestaoId = ConstantesTestePAP.PAP_QUESTAO_ID_3,
                Texto = "Avanços na aprendizagem durante o bimestre",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPResposta()
            {
                RelatorioPeriodicoQuestaoId = ConstantesTestePAP.PAP_QUESTAO_ID_4,
                Texto = "Observações",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });



            var relatorio = new RelatorioPAPDto()
            {
                PAPAlunoId = ConstantesTestePAP.PAP_ALUNO_ID_1,
                PAPTurmaId = ConstantesTestePAP.PAP_TURMA_ID_1,
                AlunoCodigo = CODIGO_ALUNO_1,
                AlunoNome = "Pap",
                periodoRelatorioPAPId = ConstantesTestePAP.PERIODO_RELATORIO_PAP_ID_1,
                TurmaId = TURMA_ID_1,
                Secoes = new List<RelatorioPAPSecaoDto>
                {
                    new RelatorioPAPSecaoDto()
                    {
                        Id = ConstantesTestePAP.PAP_SECAO_ID_1,
                        SecaoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_DIFICULDADES_APRESENTADAS_ID_2,
                        Respostas = new List<RelatorioPAPRespostaDto>()
                        {
                            new RelatorioPAPRespostaDto()
                            {
                                RelatorioRespostaId = ConstantesTestePAP.PAP_RESPOSTA_ID_1,
                                QuestaoId = ConstantesTestePAP.QUESTAO_DIFICULDADES_APRESENTADAS_ID_2,
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                                Resposta = ConstantesTestePAP.OPCAO_RESPOSTA_LEITURA_ID
                            },
                            new RelatorioPAPRespostaDto()
                            {
                                RelatorioRespostaId = ConstantesTestePAP.PAP_RESPOSTA_ID_2,
                                QuestaoId = ConstantesTestePAP.QUESTAO_OBSERVACAO_ID_3,
                                TipoQuestao = TipoQuestao.EditorTexto,
                                Resposta = "ALTERAÇÃO: Observação dificuldades apresentadas"
                            }
                        }
                    },
                    new RelatorioPAPSecaoDto()
                    {
                        Id = ConstantesTestePAP.PAP_SECAO_ID_2,
                        SecaoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_AVANC_APREND_BIMES_ID_3,
                        Respostas = new List<RelatorioPAPRespostaDto>()
                        {
                            new RelatorioPAPRespostaDto()
                            {

                                RelatorioRespostaId = ConstantesTestePAP.PAP_RESPOSTA_ID_3,
                                QuestaoId = ConstantesTestePAP.QUESTAO_AVANÇOS_NA_APRENDIZAGEM_DURANTE_O_BIMESTRE_ID_4,
                                TipoQuestao = TipoQuestao.EditorTexto,
                                Resposta = "ALTERAÇÃO: Avanços na aprendizagem durante o bimestre"
                            }
                        }
                    },
                    new RelatorioPAPSecaoDto()
                    {
                        Id = ConstantesTestePAP.PAP_SECAO_ID_3,
                        SecaoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_OBS_ID_4,
                        Respostas = new List<RelatorioPAPRespostaDto>()
                        {
                            new RelatorioPAPRespostaDto()
                            {
                                RelatorioRespostaId = ConstantesTestePAP.PAP_RESPOSTA_ID_4,
                                QuestaoId = ConstantesTestePAP.QUESTAO_OBSERVACOES_ID_5,
                                TipoQuestao = TipoQuestao.EditorTexto,
                                Resposta = "ALTERAÇÃO: Observações"
                            }
                        }
                    }
                }
            };

            var useCase = ServiceProvider.GetService<ISalvarRelatorioPAPUseCase>();

            var retorno = await useCase.Executar(relatorio);

            retorno.ShouldNotBeNull();

            var relatorioResposta = ObterTodos<RelatorioPeriodicoPAPResposta>();
            relatorioResposta.ShouldNotBeNull();
            relatorioResposta.Count.ShouldBe(4);

            var relatorioQuestao = ObterTodos<RelatorioPeriodicoPAPQuestao>();
            relatorioQuestao.ShouldNotBeNull();
            relatorioQuestao.Count.ShouldBe(4);

            var questaoDificuldade = relatorioQuestao.Find(a => a.QuestaoId == ConstantesTestePAP.QUESTAO_DIFICULDADES_APRESENTADAS_ID_2);
            questaoDificuldade.ShouldNotBeNull();

            var questaoDificuldadeObservacao = relatorioQuestao.Find(a => a.QuestaoId == ConstantesTestePAP.QUESTAO_OBSERVACAO_ID_3);
            questaoDificuldadeObservacao.ShouldNotBeNull();

            var questaoAvancos = relatorioQuestao.Find(a => a.QuestaoId == ConstantesTestePAP.QUESTAO_AVANÇOS_NA_APRENDIZAGEM_DURANTE_O_BIMESTRE_ID_4);
            questaoAvancos.ShouldNotBeNull();

            var questaoObsevacao = relatorioQuestao.Find(a => a.QuestaoId == ConstantesTestePAP.QUESTAO_OBSERVACOES_ID_5);
            questaoObsevacao.ShouldNotBeNull();

            var respostaDificuldade = relatorioResposta.Find(r => r.RelatorioPeriodicoQuestaoId == questaoDificuldade.Id);
            respostaDificuldade.ShouldNotBeNull();
            respostaDificuldade.RespostaId.ShouldNotBeNull();
            respostaDificuldade.RespostaId.ShouldBe(long.Parse(ConstantesTestePAP.OPCAO_RESPOSTA_LEITURA_ID));

            var respostaDificuldadeObservacao = relatorioResposta.Find(r => r.RelatorioPeriodicoQuestaoId == questaoDificuldadeObservacao.Id);
            respostaDificuldadeObservacao.ShouldNotBeNull();
            respostaDificuldadeObservacao.Texto.ShouldBe("ALTERAÇÃO: Observação dificuldades apresentadas");

            var respostaQuestaoAvancos = relatorioResposta.Find(r => r.RelatorioPeriodicoQuestaoId == questaoAvancos.Id);
            respostaQuestaoAvancos.ShouldNotBeNull();
            respostaQuestaoAvancos.Texto.ShouldBe("ALTERAÇÃO: Avanços na aprendizagem durante o bimestre");

            var respostaObsevacao = relatorioResposta.Find(r => r.RelatorioPeriodicoQuestaoId == questaoObsevacao.Id);
            respostaObsevacao.ShouldNotBeNull();
            respostaObsevacao.Texto.ShouldBe("ALTERAÇÃO: Observações");
        }
    }
}
