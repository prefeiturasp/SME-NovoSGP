using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.ExcluirTurmaExtinta.ServicosFakes;
using Xunit;
using PeriodoFechamentoBimestre = SME.SGP.Dominio.PeriodoFechamentoBimestre;

namespace SME.SGP.TesteIntegracao
{
    public class Ao_validar_periodo_fechamento_abertura_reabertura : TesteBaseComuns
    {
        public Ao_validar_periodo_fechamento_abertura_reabertura(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery, TurmaParaSyncInstitucionalDto>), typeof(ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQueryFake), ServiceLifetime.Scoped));
        }
        
        [Fact(DisplayName = "Período Aberto - Deve retornar true quando estiver dentro do período de fechamento")]
        public async Task Deve_retornar_true_quando_estiver_dentro_do_periodo_de_fechamento()
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;
            
            await CriarDreUe(DRE_CODIGO_1, UE_CODIGO_1);
            
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            
            await CriarTurma(Modalidade.Fundamental, ANO_8);
            
            await CriarPeriodoEscolarCustomizadoQuartoBimestre();

            await CriarPeriodoFechamento(dataReferencia);
            
            var useCase = ServiceProvider.GetService<IPeriodoFechamentoUseCase>();

            var retorno = await useCase.Executar(TURMA_CODIGO_1,dataReferencia,BIMESTRE_4);

            retorno.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Período Aberto - Não deve retornar true quando estiver fora do período de fechamento")]
        public async Task Deve_retornar_true_quando_estiver_estiver_fora_do_periodo_de_fechamento()
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;
            
            await CriarDreUe(DRE_CODIGO_1, UE_CODIGO_1);
            
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            
            await CriarTurma(Modalidade.Fundamental, ANO_8);
            
            await CriarPeriodoEscolarCustomizadoQuartoBimestre();

            await CriarPeriodoFechamento(dataReferencia);
            
            var useCase = ServiceProvider.GetService<IPeriodoFechamentoUseCase>();

            var retorno = await useCase.Executar(TURMA_CODIGO_1,dataReferencia,BIMESTRE_2);

            retorno.ShouldBeFalse();
        }

        [Fact(DisplayName = "Período Aberto - Deve retornar true quando estiver dentro do período de fechamento reabertura")]
        public async Task Deve_retornar_true_quando_estiver_dentro_do_periodo_de_fechamento_reabertura()
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;
            
            await CriarDreUe(DRE_CODIGO_1, UE_CODIGO_1);
            
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            
            await CriarTurma(Modalidade.Fundamental, ANO_8);
            
            await CriarPeriodoEscolarCustomizadoQuartoBimestre();

            await CriarPeriodoFechamento(dataReferencia.AddDays(-50));

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
            
            var useCase = ServiceProvider.GetService<IPeriodoFechamentoUseCase>();

            var retorno = await useCase.Executar(TURMA_CODIGO_1,dataReferencia,BIMESTRE_4);

            retorno.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Período Aberto - Não deve retornar true quando estiver fora do período de fechamento")]
        public async Task Nao_deve_retornar_true_quando_estiver_estiver_fora_do_periodo_de_fechamento()
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;
            
            await CriarDreUe(DRE_CODIGO_1, UE_CODIGO_1);
            
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            
            await CriarTurma(Modalidade.Fundamental, ANO_8);
            
            await CriarPeriodoEscolarCustomizadoQuartoBimestre();
        
            await CriarPeriodoFechamento(dataReferencia.AddDays(-50));
            
            await CriarPeriodoReabertura(TIPO_CALENDARIO_1, dataReferencia.AddDays(-10).Date, dataReferencia.AddDays(-1).Date);
            
            var useCase = ServiceProvider.GetService<IPeriodoFechamentoUseCase>();
        
            var retorno = await useCase.Executar(TURMA_CODIGO_1,dataReferencia,BIMESTRE_4);
        
            retorno.ShouldBeFalse();
        }
        private async Task CriarPeriodoFechamento(DateTime dataReferencia)
        {
            await InserirNaBase(new PeriodoFechamento()
            {
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                PeriodoFechamentoId = PERIODO_FECHAMENTO_ID_1,
                InicioDoFechamento = dataReferencia.AddDays(-40).Date,
                FinalDoFechamento = dataReferencia.AddDays(-31).Date
            });
            
            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_2,
                PeriodoFechamentoId = PERIODO_FECHAMENTO_ID_1,
                InicioDoFechamento = dataReferencia.AddDays(-30).Date,
                FinalDoFechamento = dataReferencia.AddDays(-21).Date
            });
            
            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_3,
                PeriodoFechamentoId = PERIODO_FECHAMENTO_ID_1,
                InicioDoFechamento = dataReferencia.AddDays(-20).Date,
                FinalDoFechamento = dataReferencia.AddDays(-9).Date
            });
            
            await InserirNaBase(new PeriodoFechamentoBimestre()
            {
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_4,
                PeriodoFechamentoId = PERIODO_FECHAMENTO_ID_1,
                InicioDoFechamento = dataReferencia.AddDays(-10).Date,
                FinalDoFechamento = dataReferencia.AddDays(+10).Date
            });
        }
    }
}
