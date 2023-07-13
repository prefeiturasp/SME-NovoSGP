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
    public class Ao_obter_periodos_pap : RelatorioPAPTesteBase
    {
        public Ao_obter_periodos_pap(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_obter_peridos_semestral()
        {
            await CriarDadosBase();

            await InserirNaBase(new ConfiguracaoRelatorioPAP()
            {
                Id = 1,
                InicioVigencia = DATA_03_01_INICIO_BIMESTRE_1,
                FimVigencia = DATA_01_05_FIM_BIMESTRE_1,
                TipoPeriocidade = ConstantesTestePAP.TIPO_PERIODICIDADE_SEMANAL,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new PeriodoRelatorioPAP()
            {
                ConfiguracaoId = 1,
                Periodo = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new PeriodoEscolarRelatorioPAP()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                PeriodoRelatorioId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new PeriodoEscolarRelatorioPAP()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_2,
                PeriodoRelatorioId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterPeriodosPAPUseCase>();
            var periodos = await useCase.Executar(TURMA_CODIGO_1);

            periodos.ShouldNotBeNull();

            periodos.Count().ShouldBe(1);
            var periodo = periodos.FirstOrDefault();
            periodo.PeriodoRelatorioPAPId.ShouldBe(1);
            periodo.TipoConfiguracaoPeriodicaRelatorioPAP.ShouldBe(ConstantesTestePAP.TIPO_PERIODICIDADE_SEMANAL.ToString());
            periodo.PeriodoRelatorioPAP.ShouldBe(1);
        }

        [Fact]
        public async Task Ao_obter_peridos_bimestral()
        {
            await CriarDadosBase();

            await InserirNaBase(new ConfiguracaoRelatorioPAP()
            {
                Id = 1,
                InicioVigencia = DATA_03_01_INICIO_BIMESTRE_1,
                FimVigencia = DATA_01_05_FIM_BIMESTRE_1,
                TipoPeriocidade = ConstantesTestePAP.TIPO_PERIODICIDADE_BIMESTRAL,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new PeriodoRelatorioPAP()
            {
                ConfiguracaoId = 1,
                Periodo = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new PeriodoEscolarRelatorioPAP()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                PeriodoRelatorioId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new PeriodoRelatorioPAP()
            {
                ConfiguracaoId = 1,
                Periodo = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new PeriodoEscolarRelatorioPAP()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_2,
                PeriodoRelatorioId = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterPeriodosPAPUseCase>();
            var periodos = await useCase.Executar(TURMA_CODIGO_1);

            periodos.ShouldNotBeNull();

            periodos.Count().ShouldBe(2);
            var periodo1 = periodos.FirstOrDefault();
            periodo1.PeriodoRelatorioPAPId.ShouldBe(1);
            periodo1.TipoConfiguracaoPeriodicaRelatorioPAP.ShouldBe(ConstantesTestePAP.TIPO_PERIODICIDADE_BIMESTRAL.ToString());
            periodo1.PeriodoRelatorioPAP.ShouldBe(1);
            var periodo2 = periodos.LastOrDefault();
            periodo2.PeriodoRelatorioPAPId.ShouldBe(2);
            periodo2.TipoConfiguracaoPeriodicaRelatorioPAP.ShouldBe(ConstantesTestePAP.TIPO_PERIODICIDADE_BIMESTRAL.ToString());
            periodo2.PeriodoRelatorioPAP.ShouldBe(2);
        }
    }
}
