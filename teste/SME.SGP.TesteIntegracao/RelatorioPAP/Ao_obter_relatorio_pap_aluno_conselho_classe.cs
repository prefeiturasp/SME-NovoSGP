using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.RelatorioPAP.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RelatorioPAP
{
    public class Ao_obter_relatorio_pap_aluno_conselho_classe : RelatorioPAPTesteBase
    {
        public Ao_obter_relatorio_pap_aluno_conselho_classe(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Obter o relatório pap do aluno para conselho de classe")]
        public async Task Ao_obter_questionario_com_resposta_semestre_anterior()
        {
            await CriarDadosBase(true, true);

            await InserirNaBase(new RelatorioPeriodicoPAPTurma()
            {
                Id = 1,
                PeriodoRelatorioId = ConstantesTestePAP.CONFIGURACAO_RELATORIO_PAP_ID_1,
                TurmaId = TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPAluno()
            {
                CodigoAluno = CODIGO_ALUNO_1,
                NomeAluno = "Jailsin das Oliveiras",
                RelatorioPeriodicoTurmaId = ConstantesTestePAP.RELATORIO_PERIODICO_TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });;
            await InserirSecoesQuestoes();
            
            var useCase = ServiceProvider.GetService<IObterRelatorioPAPConselhoClasseUseCase>();
            var secoesQuestoes = await useCase.Executar(TURMA_CODIGO_1, CODIGO_ALUNO_1, 1);

            secoesQuestoes.ShouldNotBeNull();
            secoesQuestoes.Count().ShouldBe(3);
            var secaoFrequencia = secoesQuestoes.ToList().Find(secao => secao.NomeComponente == ConstantesTestePAP.NOME_COMPONENTE_SECAO_FREQUENCIA);
            secaoFrequencia.ShouldBeNull();

            var secaoDificuldadesApresentadas = secoesQuestoes.ToList().Find(secao => secao.NomeComponente == ConstantesTestePAP.NOME_COMPONENTE_SECAO_DIFIC_APRES);
            secaoDificuldadesApresentadas.ShouldNotBeNull();
            secaoDificuldadesApresentadas.Questoes.Count().ShouldBe(2);
            var questao = secaoDificuldadesApresentadas.Questoes.Where(q => q.Id.Equals(ConstantesTestePAP.QUESTAO_DIFICULDADES_APRESENTADAS_ID_2)).FirstOrDefault();
            questao.ShouldNotBeNull("Questão Dificuldades Apresentadas Ordem 1 não encontrada");
            questao.Resposta.Where(r => r.OpcaoRespostaId.Equals(ConstantesTestePAP.OPCAO_RESPOSTA_ESCRITA_ID)
                                        || r.OpcaoRespostaId.Equals(long.Parse(ConstantesTestePAP.OPCAO_RESPOSTA_LEITURA_ID))).Count().ShouldBe(2);
            questao = secaoDificuldadesApresentadas.Questoes.Where(q => q.Id.Equals(ConstantesTestePAP.QUESTAO_OBSERVACAO_ID_3)).FirstOrDefault();
            questao.ShouldNotBeNull("Questão Observações (Dificuldades Apresentadas) Ordem 2 não encontrada");
            questao.Resposta.Any(r => r.Texto.Equals("Resposta Observações (Dificuldades apresentadas)")).ShouldBeTrue();

            var secaoAvancosDuranteBimestre = secoesQuestoes.ToList().Find(secao => secao.NomeComponente == ConstantesTestePAP.NOME_COMPONENTE_SECAO_AVANC_APREND_BIMES);
            secaoAvancosDuranteBimestre.ShouldNotBeNull();
            secaoAvancosDuranteBimestre.Questoes.Count().ShouldBe(1);
            questao = secaoAvancosDuranteBimestre.Questoes.Where(q => q.Id.Equals(ConstantesTestePAP.QUESTAO_AVANÇOS_NA_APRENDIZAGEM_DURANTE_O_BIMESTRE_ID_4)).FirstOrDefault();
            questao.ShouldNotBeNull("Questão Avanços na aprendizagem durante o bimestre Ordem 1 não encontrada");
            questao.Resposta.Any(r => r.Texto.Equals("Resposta Avanços na aprendizagem durante o bimestre")).ShouldBeTrue();

            var secaoObservacoes = secoesQuestoes.ToList().Find(secao => secao.NomeComponente == ConstantesTestePAP.NOME_COMPONENTE_SECAO_OBS);
            secaoObservacoes.ShouldNotBeNull();
            secaoObservacoes.Questoes.Count().ShouldBe(1);
            questao = secaoObservacoes.Questoes.Where(q => q.Id.Equals(ConstantesTestePAP.QUESTAO_OBSERVACOES_ID_5)).FirstOrDefault();
            questao.ShouldNotBeNull("Questão Observações Ordem 1 não encontrada");
            questao.Resposta.Any(r => r.Texto.Equals("Resposta Observações")).ShouldBeTrue();
        }

        private async Task InserirSecoesQuestoes()
        {
            //Frequencia
            await InserirNaBase(new RelatorioPeriodicoPAPSecao()
            {
                RelatorioPeriodicoAlunoId = ConstantesTestePAP.RELATORIO_PERIODICO_ALUNO_ID_1,
                SecaoRelatorioPeriodicoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_FREQUENCIA_NA_TURMA_PAP_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPQuestao()
            {
                RelatorioPeriodiocoSecaoId = ObterUltimoId<RelatorioPeriodicoPAPSecao>(),
                QuestaoId = ConstantesTestePAP.QUESTAO_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPResposta()
            {
                RelatorioPeriodicoQuestaoId = ObterUltimoId<RelatorioPeriodicoPAPQuestao>(),
                Texto = "teste resposta",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //Dificuldades apresentadas
            await InserirNaBase(new RelatorioPeriodicoPAPSecao()
            {
                RelatorioPeriodicoAlunoId = ConstantesTestePAP.RELATORIO_PERIODICO_ALUNO_ID_1,
                SecaoRelatorioPeriodicoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_DIFICULDADES_APRESENTADAS_ID_2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPQuestao()
            {
                RelatorioPeriodiocoSecaoId = ObterUltimoId<RelatorioPeriodicoPAPSecao>(),
                QuestaoId = ConstantesTestePAP.QUESTAO_DIFICULDADES_APRESENTADAS_ID_2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPResposta()
            {
                RelatorioPeriodicoQuestaoId = ObterUltimoId<RelatorioPeriodicoPAPQuestao>(),
                Texto = "",
                RespostaId = ConstantesTestePAP.OPCAO_RESPOSTA_ESCRITA_ID,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPResposta()
            {
                RelatorioPeriodicoQuestaoId = ObterUltimoId<RelatorioPeriodicoPAPQuestao>(),
                Texto = "",
                RespostaId = long.Parse(ConstantesTestePAP.OPCAO_RESPOSTA_LEITURA_ID),
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPQuestao()
            {
                RelatorioPeriodiocoSecaoId = ObterUltimoId<RelatorioPeriodicoPAPSecao>(),
                QuestaoId = ConstantesTestePAP.QUESTAO_OBSERVACAO_ID_3,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPResposta()
            {
                RelatorioPeriodicoQuestaoId = ObterUltimoId<RelatorioPeriodicoPAPQuestao>(),
                Texto = "Resposta Observações (Dificuldades apresentadas)",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //Avanços na aprendizagem durante o bimestre
            await InserirNaBase(new RelatorioPeriodicoPAPSecao()
            {
                RelatorioPeriodicoAlunoId = ConstantesTestePAP.RELATORIO_PERIODICO_ALUNO_ID_1,
                SecaoRelatorioPeriodicoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_AVANC_APREND_BIMES_ID_3,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPQuestao()
            {
                RelatorioPeriodiocoSecaoId = ObterUltimoId<RelatorioPeriodicoPAPSecao>(),
                QuestaoId = ConstantesTestePAP.QUESTAO_AVANÇOS_NA_APRENDIZAGEM_DURANTE_O_BIMESTRE_ID_4,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPResposta()
            {
                RelatorioPeriodicoQuestaoId = ObterUltimoId<RelatorioPeriodicoPAPQuestao>(),
                Texto = "Resposta Avanços na aprendizagem durante o bimestre",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //Observações
            await InserirNaBase(new RelatorioPeriodicoPAPSecao()
            {
                RelatorioPeriodicoAlunoId = ConstantesTestePAP.RELATORIO_PERIODICO_ALUNO_ID_1,
                SecaoRelatorioPeriodicoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_OBS_ID_4,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPQuestao()
            {
                RelatorioPeriodiocoSecaoId = ObterUltimoId<RelatorioPeriodicoPAPSecao>(),
                QuestaoId = ConstantesTestePAP.QUESTAO_OBSERVACOES_ID_5,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPResposta()
            {
                RelatorioPeriodicoQuestaoId = ObterUltimoId<RelatorioPeriodicoPAPQuestao>(),
                Texto = "Resposta Observações",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

    }
}
