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
    public class Ao_obter_alunos_periodos_pap : RelatorioPAPTesteBase
    {
        public Ao_obter_alunos_periodos_pap(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Obter alunos por semestre pap")]
        public async Task Ao_obter_alunos_por_semestre()
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
                CodigoAluno = CODIGO_ALUNO_1,
                NomeAluno = "Joooooose",
                RelatorioPeriodicoTurmaId = ConstantesTestePAP.RELATORIO_PERIODICO_TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterAlunosPorPeriodoPAPUseCase>();
            var alunos = await useCase.Executar(TURMA_CODIGO_1, ConstantesTestePAP.CONFIGURACAO_RELATORIO_PAP_ID_1);

            alunos.ShouldNotBeNull();
            var aluno1 = alunos.FirstOrDefault(aluno => aluno.CodigoEOL == CODIGO_ALUNO_1);
            aluno1.ShouldNotBeNull();
            aluno1.Marcador.ShouldBeNull();
            aluno1.ProcessoConcluido.ShouldBeTrue();
            var aluno3 = alunos.FirstOrDefault(aluno => aluno.CodigoEOL == CODIGO_ALUNO_3);
            aluno3.ShouldNotBeNull();
            aluno3.Marcador.Tipo.ShouldBe(TipoMarcadorFrequencia.Inativo);
        }

        [Fact(DisplayName = "Obter alunos por bimestre pap")]
        public async Task Ao_obter_alunos_por_bimestre()
        {
            await CriarDadosBase(true, true);

            await InserirNaBase(new ConfiguracaoRelatorioPAP()
            {
                InicioVigencia = DATA_01_01_INICIO_BIMESTRE_1,
                FimVigencia = DATA_01_05_FIM_BIMESTRE_1,
                TipoPeriocidade = ConstantesTestePAP.TIPO_PERIODICIDADE_BIMESTRAL,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new PeriodoRelatorioPAP()
            {
                ConfiguracaoId = ConstantesTestePAP.CONFIGURACAO_RELATORIO_PAP_ID_2,
                Periodo = BIMESTRE_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new PeriodoEscolarRelatorioPAP()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                PeriodoRelatorioId = ConstantesTestePAP.PERIODO_RELATORIO_PAP_ID_2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });


            await InserirNaBase(new RelatorioPeriodicoPAPTurma()
            {
                PeriodoRelatorioId = ConstantesTestePAP.PERIODO_RELATORIO_PAP_ID_2,
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
            });

            var useCase = ServiceProvider.GetService<IObterAlunosPorPeriodoPAPUseCase>();
            var alunos = await useCase.Executar(TURMA_CODIGO_1, ConstantesTestePAP.CONFIGURACAO_RELATORIO_PAP_ID_2);

            alunos.ShouldNotBeNull();
            var aluno1 = alunos.FirstOrDefault(aluno => aluno.CodigoEOL == CODIGO_ALUNO_1);
            aluno1.ShouldNotBeNull();
            aluno1.Marcador.ShouldBeNull();
            aluno1.ProcessoConcluido.ShouldBeTrue();
            var aluno3 = alunos.FirstOrDefault(aluno => aluno.CodigoEOL == CODIGO_ALUNO_3);
            aluno3.ShouldNotBeNull();
            aluno3.Marcador.Tipo.ShouldBe(TipoMarcadorFrequencia.Inativo);
            aluno3.ProcessoConcluido.ShouldBeFalse();
        }
    }
}
