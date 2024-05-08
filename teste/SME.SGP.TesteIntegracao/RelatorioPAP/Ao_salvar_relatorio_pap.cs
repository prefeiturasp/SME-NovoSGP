using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Ocsp;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
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
    public class Ao_salvar_relatorio_pap : RelatorioPAPTesteBase
    {
        public Ao_salvar_relatorio_pap(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Salvar relatório pap para aluno")]
        public async Task Ao_salvar_relatorio_pap_aluno()
        {
            await CriarDadosBase(true, true);

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

            var retorno = await useCase.Executar(relatorio);

            retorno.ShouldNotBeNull();
            retorno.PAPAlunoId.ShouldBe(ConstantesTestePAP.PAP_ALUNO_ID_1);
            retorno.PAPTurmaId.ShouldBe(ConstantesTestePAP.PAP_TURMA_ID_1);
            retorno.Secoes.ShouldNotBeNull();
            retorno.Secoes.Count().ShouldBe(3);

            var relatorioTurma = ObterTodos<RelatorioPeriodicoPAPTurma>();
            relatorioTurma.ShouldNotBeNull();
            relatorioTurma.Count.ShouldBe(1);
            relatorioTurma.Any(a => a.TurmaId == TURMA_ID_1 && a.PeriodoRelatorioId == ConstantesTestePAP.PERIODO_RELATORIO_PAP_ID_1).ShouldBeTrue();

            var relatorioAluno = ObterTodos<RelatorioPeriodicoPAPAluno>();
            relatorioAluno.ShouldNotBeNull();
            relatorioAluno.Count.ShouldBe(1);
            relatorioAluno.Any(a => a.CodigoAluno == CODIGO_ALUNO_1 && a.NomeAluno == "Pap").ShouldBeTrue();

            var relatorioSecao = ObterTodos<RelatorioPeriodicoPAPSecao>();
            relatorioSecao.ShouldNotBeNull();
            relatorioSecao.Count.ShouldBe(3);
            relatorioSecao.Any(a => a.SecaoRelatorioPeriodicoId == ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_DIFICULDADES_APRESENTADAS_ID_2).ShouldBeTrue();
            relatorioSecao.Any(a => a.SecaoRelatorioPeriodicoId == ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_AVANC_APREND_BIMES_ID_3).ShouldBeTrue();
            relatorioSecao.Any(a => a.SecaoRelatorioPeriodicoId == ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_OBS_ID_4).ShouldBeTrue();

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

            var relatorioResposta = ObterTodos<RelatorioPeriodicoPAPResposta>();
            relatorioResposta.ShouldNotBeNull();
            relatorioResposta.Count.ShouldBe(4);

            var respostaDificuldade = relatorioResposta.Find(r => r.RelatorioPeriodicoQuestaoId == questaoDificuldade.Id);
            respostaDificuldade.ShouldNotBeNull();
            respostaDificuldade.RespostaId.ShouldNotBeNull();
            respostaDificuldade.RespostaId.ShouldBe(long.Parse(ConstantesTestePAP.OPCAO_RESPOSTA_LEITURA_ID));

            var respostaDificuldadeObservacao = relatorioResposta.Find(r => r.RelatorioPeriodicoQuestaoId == questaoDificuldadeObservacao.Id);
            respostaDificuldadeObservacao.ShouldNotBeNull();
            respostaDificuldadeObservacao.Texto.ShouldBe("Observação dificuldades apresentadas");

            var respostaQuestaoAvancos = relatorioResposta.Find(r => r.RelatorioPeriodicoQuestaoId == questaoAvancos.Id);
            respostaQuestaoAvancos.ShouldNotBeNull();
            respostaQuestaoAvancos.Texto.ShouldBe("Avanços na aprendizagem durante o bimestre");

            var respostaObsevacao = relatorioResposta.Find(r => r.RelatorioPeriodicoQuestaoId == questaoObsevacao.Id);
            respostaObsevacao.ShouldNotBeNull();
            respostaObsevacao.Texto.ShouldBe("Observações");
        }

        [Fact(DisplayName = "Salvar relatório pap para aluno validar questões obrigatórias")]
        public async Task Ao_salvar_relatorio_pap_aluno_validar_questoes_obrigatorias()
        {
            await CriarDadosBase(true, true);

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

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(relatorio));

            excecao.Message.ShouldNotBeNull();

            excecao.Message.ShouldBe(string.Format(MensagemNegocioRelatorioPAP.EXISTEM_QUESTOES_OBRIGATORIAS_NAO_PREENCHIDAS, "Seção: Dificuldades apresentadas Questões: [1], Seção: Avanços na aprendizagem durante o bimestre Questões: [1]"));
        }
    }
}
