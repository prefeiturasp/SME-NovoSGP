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
    public class Ao_obter_secoes_questionario : RelatorioPAPTesteBase
    {
        public Ao_obter_secoes_questionario(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Obter todos as seçoes")]
        public async Task Ao_obter_secoes()
        {
            await CriarDadosBase(true, true);

            var useCase = ServiceProvider.GetService<IObterSecoesPAPUseCase>();
            var secoes = await useCase.Executar(new Infra.FiltroObterSecoesDto(CODIGO_ALUNO_1, TURMA_CODIGO_1, 1));

            secoes.ShouldNotBeNull();
            secoes.Secoes.ShouldNotBeNull();
            secoes.Secoes.Count().ShouldBe(4);
            secoes.Secoes.Exists(secao => secao.Nome == ConstantesTestePAP.FREQUENCIA_NA_TURMA_PAP).ShouldBeTrue();
            secoes.Secoes.Exists(secao => secao.Nome == ConstantesTestePAP.AVANÇOS_NA_APRENDIZAGEM_DURANTE_O_BIMESTRE).ShouldBeTrue();
            secoes.Secoes.Exists(secao => secao.Nome == ConstantesTestePAP.DIFICULDADES_APRESENTADAS).ShouldBeTrue();
            secoes.Secoes.Exists(secao => secao.Nome == ConstantesTestePAP.OBSERVACOES).ShouldBeTrue();
        }

        [Fact(DisplayName = "Obter todos as seçoes com auditoria")]
        public async Task Ao_obter_secoes_com_auditoria()
        {
            await CriarDadosBase(true, true);

            await InserirNaBase(new RelatorioPeriodicoPAPTurma()
            {
                Id = 1,
                PeriodoRelatorioId = 1,
                TurmaId = TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPAluno()
            {
                Id = 1,
                CodigoAluno = CODIGO_ALUNO_1,
                NomeAluno = "Joooooose",
                RelatorioPeriodicoTurmaId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPSecao()
            {
                Id = 1,
                RelatorioPeriodicoAlunoId = 1,
                SecaoRelatorioPeriodicoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_AVANC_APREND_BIMES_ID_3,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterSecoesPAPUseCase>();
            var secoes = await useCase.Executar(new Infra.FiltroObterSecoesDto(CODIGO_ALUNO_1, TURMA_CODIGO_1, 1));

            secoes.ShouldNotBeNull();
            secoes.Secoes.ShouldNotBeNull();
            secoes.Secoes.Count().ShouldBe(4);
            
            var secaoAvanco = secoes.Secoes.Find(secao => secao.Nome == ConstantesTestePAP.AVANÇOS_NA_APRENDIZAGEM_DURANTE_O_BIMESTRE);
            secaoAvanco.ShouldNotBeNull();
            secaoAvanco.Auditoria.ShouldNotBeNull();

            secoes.Secoes.Exists(secao => secao.Nome == ConstantesTestePAP.FREQUENCIA_NA_TURMA_PAP).ShouldBeTrue();
            secoes.Secoes.Exists(secao => secao.Nome == ConstantesTestePAP.DIFICULDADES_APRESENTADAS).ShouldBeTrue();
            secoes.Secoes.Exists(secao => secao.Nome == ConstantesTestePAP.OBSERVACOES).ShouldBeTrue();
        }


        [Fact(DisplayName = "Obter o questionario")]
        public async Task Ao_obter_questionario()
        {
            await CriarDadosBase(true, true);

            var useCase = ServiceProvider.GetService<IObterQuestionarioPAPUseCase>();
            var questoes = await useCase.Executar(TURMA_CODIGO_1, CODIGO_ALUNO_1, ConstantesTestePAP.PERIODO_RELATORIO_PAP_ID_1, ConstantesTestePAP.QUESTIONARIO_AVANC_APREND_BIMES_ID, null);

            questoes.ShouldNotBeNull();
            questoes.Count().ShouldBe(1);
            questoes.ToList().Exists(questao => questao.NomeComponente == ConstantesTestePAP.NOME_COMPONENTE_AVANC_APREND_BIMES).ShouldBeTrue();
        }

        [Fact(DisplayName = "Obter o questionario com resposta")]
        public async Task Ao_obter_questionario_com_resposta()
        {
            await CriarDadosBase(true, true);

            await InserirNaBase(new RelatorioPeriodicoPAPTurma()
            {
                Id = 1,
                PeriodoRelatorioId = 1,
                TurmaId = TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPAluno()
            {
                Id = 1,
                CodigoAluno = CODIGO_ALUNO_1,
                NomeAluno = "Joooooose",
                RelatorioPeriodicoTurmaId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPSecao()
            {
                Id = 1,
                RelatorioPeriodicoAlunoId = 1,
                SecaoRelatorioPeriodicoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_AVANC_APREND_BIMES_ID_3,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPQuestao()
            {
                RelatorioPeriodiocoSecaoId = 1,
                QuestaoId = ConstantesTestePAP.QUESTAO_AVANÇOS_NA_APRENDIZAGEM_DURANTE_O_BIMESTRE_ID_4,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPResposta() 
            {
                RelatorioPeriodicoQuestaoId = 1,
                Texto = "teste resposta",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterQuestionarioPAPUseCase>();
            var questoes = await useCase.Executar(TURMA_CODIGO_1, CODIGO_ALUNO_1, ConstantesTestePAP.PERIODO_RELATORIO_PAP_ID_1, ConstantesTestePAP.QUESTIONARIO_AVANC_APREND_BIMES_ID, 1);

            questoes.ShouldNotBeNull();
            questoes.Count().ShouldBe(1);
            var questao = questoes.ToList().Find(questao => questao.NomeComponente == ConstantesTestePAP.NOME_COMPONENTE_AVANC_APREND_BIMES);
            questao.ShouldNotBeNull();
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("teste resposta");
        }

        [Fact(DisplayName = "Obter o questionário com resposta do semestre anterior")]
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
                NomeAluno = "Joooooose",
                RelatorioPeriodicoTurmaId = ConstantesTestePAP.RELATORIO_PERIODICO_TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });;

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
                RelatorioPeriodiocoSecaoId = 1,
                QuestaoId = ConstantesTestePAP.QUESTAO_AVANÇOS_NA_APRENDIZAGEM_DURANTE_O_BIMESTRE_ID_4,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPResposta()
            {
                RelatorioPeriodicoQuestaoId = 1,
                Texto = "teste resposta",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await CriarConfiguracaoRelatorio(DATA_25_07_INICIO_BIMESTRE_3);

            await CriarPeriodoRelatorio(ConstantesTestePAP.CONFIGURACAO_RELATORIO_PAP_ID_2, ConstantesTestePAP.PERIODO_SEGUNDO_SEMESTRE);

            await CriarPeriodoEscolarRelatorio(ConstantesTestePAP.PERIODO_RELATORIO_PAP_ID_2, PERIODO_ESCOLAR_CODIGO_2);

            await CriarSecaoConfRelatorioPeriodico(ConstantesTestePAP.CONFIGURACAO_RELATORIO_PAP_ID_2);

            var useCase = ServiceProvider.GetService<IObterQuestionarioPAPUseCase>();
            var questoes = await useCase.Executar(TURMA_CODIGO_1, CODIGO_ALUNO_1, ConstantesTestePAP.PERIODO_RELATORIO_PAP_ID_2, ConstantesTestePAP.QUESTIONARIO_AVANC_APREND_BIMES_ID, null);

            questoes.ShouldNotBeNull();
            questoes.Count().ShouldBe(1);
            var questao = questoes.ToList().Find(questao => questao.NomeComponente == ConstantesTestePAP.NOME_COMPONENTE_AVANC_APREND_BIMES);
            questao.ShouldNotBeNull();
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("teste resposta");
        }

        [Fact(DisplayName = "Obter o questionário com resposta do bimestre anterior")]
        public async Task Ao_obter_questionario_com_resposta_bimestre_anterior()
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
                NomeAluno = "Joooooose",
                RelatorioPeriodicoTurmaId = ConstantesTestePAP.RELATORIO_PERIODICO_TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }); ;

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
                RelatorioPeriodiocoSecaoId = 1,
                QuestaoId = ConstantesTestePAP.QUESTAO_AVANÇOS_NA_APRENDIZAGEM_DURANTE_O_BIMESTRE_ID_4,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPResposta()
            {
                RelatorioPeriodicoQuestaoId = 1,
                Texto = "teste resposta",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await CriarConfiguracaoRelatorio(DATA_02_05_INICIO_BIMESTRE_2);

            await CriarPeriodoRelatorio(ConstantesTestePAP.CONFIGURACAO_RELATORIO_PAP_ID_2, BIMESTRE_2);

            await CriarPeriodoEscolarRelatorio(ConstantesTestePAP.PERIODO_RELATORIO_PAP_ID_2, PERIODO_ESCOLAR_CODIGO_1);

            await CriarSecaoConfRelatorioPeriodico(ConstantesTestePAP.CONFIGURACAO_RELATORIO_PAP_ID_2);

            await InserirNaBase(new RelatorioPeriodicoPAPTurma()
            {
                Id = 1,
                PeriodoRelatorioId = ConstantesTestePAP.CONFIGURACAO_RELATORIO_PAP_ID_2,
                TurmaId = TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPAluno()
            {
                CodigoAluno = CODIGO_ALUNO_1,
                NomeAluno = "Joooooose",
                RelatorioPeriodicoTurmaId = ConstantesTestePAP.RELATORIO_PERIODICO_TURMA_ID_2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }); ;

            await InserirNaBase(new RelatorioPeriodicoPAPSecao()
            {
                RelatorioPeriodicoAlunoId = ConstantesTestePAP.RELATORIO_PERIODICO_ALUNO_ID_2,
                SecaoRelatorioPeriodicoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_AVANC_APREND_BIMES_ID_3,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPQuestao()
            {
                RelatorioPeriodiocoSecaoId = 2,
                QuestaoId = ConstantesTestePAP.QUESTAO_AVANÇOS_NA_APRENDIZAGEM_DURANTE_O_BIMESTRE_ID_4,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RelatorioPeriodicoPAPResposta()
            {
                RelatorioPeriodicoQuestaoId = 2,
                Texto = "teste resposta bimestre 2",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await CriarConfiguracaoRelatorio(DATA_25_07_INICIO_BIMESTRE_3);

            await CriarPeriodoRelatorio(ConstantesTestePAP.CONFIGURACAO_RELATORIO_PAP_ID_3, BIMESTRE_3);

            await CriarPeriodoEscolarRelatorio(ConstantesTestePAP.PERIODO_RELATORIO_PAP_ID_3, PERIODO_ESCOLAR_CODIGO_3);

            await CriarSecaoConfRelatorioPeriodico(ConstantesTestePAP.CONFIGURACAO_RELATORIO_PAP_ID_3);

            var useCase = ServiceProvider.GetService<IObterQuestionarioPAPUseCase>();
            var questoes = await useCase.Executar(TURMA_CODIGO_1, CODIGO_ALUNO_1, ConstantesTestePAP.PERIODO_RELATORIO_PAP_ID_3, ConstantesTestePAP.QUESTIONARIO_AVANC_APREND_BIMES_ID, null);

            questoes.ShouldNotBeNull();
            questoes.Count().ShouldBe(1);
            var questao = questoes.ToList().Find(questao => questao.NomeComponente == ConstantesTestePAP.NOME_COMPONENTE_AVANC_APREND_BIMES);
            questao.ShouldNotBeNull();
            questao.Resposta.FirstOrDefault().Texto.ShouldBe("teste resposta bimestre 2");
        }

        private async Task CriarPeriodoEscolarRelatorio(long periodoRelatorioId, long peridoId)
        {
            await InserirNaBase(new PeriodoEscolarRelatorioPAP()
            {
                PeriodoRelatorioId = periodoRelatorioId,
                PeriodoEscolarId = peridoId,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
