using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.PeriodoEscolarValidacoes
{
    public class Validar_periodo_escolar : TesteBaseComuns
    {
        public Validar_periodo_escolar(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        [Fact(DisplayName = "Perido Escolar - Obter último bimestre")]
        public async Task Ao_obter_ultimo_bimestre()
        {
            await CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();
            
            var repositorioPeriodoEscolarConsulta = ServiceProvider.GetService<IRepositorioPeriodoEscolarConsulta>();

            var retorno = await repositorioPeriodoEscolarConsulta.ObterUltimoBimestreAsync(ANO_LETIVO_ANO_ATUAL,ModalidadeTipoCalendario.CELP,SEMESTRE_2);
            retorno.ShouldNotBeNull();
            retorno.TipoCalendarioId.ShouldBe(TIPO_CALENDARIO_9);
            retorno.PeriodoInicio.ShouldBe(DATA_03_10_INICIO_BIMESTRE_4);
        }
        
        [Fact(DisplayName = "Perido Escolar - Obter Por Ano Letivo E Modalidade")]
        public async Task Ao_obter_por_ano_letivo_e_modalidade()
        {
            await CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();
            
            var repositorioPeriodoEscolarConsulta = ServiceProvider.GetService<IRepositorioPeriodoEscolarConsulta>();

            var retorno = await repositorioPeriodoEscolarConsulta.ObterPorAnoLetivoEModalidadeTurma(ANO_LETIVO_ANO_ATUAL,ModalidadeTipoCalendario.CELP,SEMESTRE_2);
            retorno.ShouldNotBeNull();
            var clpPrimeiroBimestre = retorno.FirstOrDefault(w => w.Bimestre == BIMESTRE_1);
            clpPrimeiroBimestre.PeriodoInicio.ShouldBe(DATA_25_07_INICIO_BIMESTRE_3);
            clpPrimeiroBimestre.TipoCalendarioId.ShouldBe(TIPO_CALENDARIO_9);
            
            var clpSegundoBimestre = retorno.FirstOrDefault(w => w.Bimestre == BIMESTRE_2);
            clpSegundoBimestre.PeriodoInicio.ShouldBe(DATA_03_10_INICIO_BIMESTRE_4);
            clpSegundoBimestre.TipoCalendarioId.ShouldBe(TIPO_CALENDARIO_9);
        }
        
        [Fact(DisplayName = "Perido Escolar - Obter Periodo Escolar Id Por Turma e Bimestre")]
        public async Task Ao_obter_periodo_escolar_id_por_turma_bimestre()
        {
            await CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();

            await CriarDreUe(DRE_CODIGO_1, UE_CODIGO_1);

            await CriarTurma(Modalidade.CELP, false, true);
            
            var repositorioPeriodoEscolarConsulta = ServiceProvider.GetService<IRepositorioPeriodoEscolarConsulta>();

            var retorno = await repositorioPeriodoEscolarConsulta.ObterPeriodoEscolarIdPorTurmaBimestre(TURMA_CODIGO_1, ModalidadeTipoCalendario.CELP,BIMESTRE_1, ANO_LETIVO_ANO_ATUAL,SEMESTRE_2);
            retorno.ShouldBeGreaterThan(0);
            retorno.ShouldBeGreaterThan(13);
        }
        
        [Fact(DisplayName = "Perido Escolar - Obter Ues Com Dre Por Codigo E Modalidade")]
        public async Task Ao_obter_ues_com_dre_por_codigo_e_modalidade()
        {
            await CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();

            await CriarDreUe(DRE_CODIGO_1, UE_CODIGO_1);

            await CriarTurma(Modalidade.CELP, false, true);
            
            var mediator = ServiceProvider.GetService<IMediator>();

            var modalidades = new [] { Modalidade.CELP, Modalidade.EJA };

            var retorno = await mediator.Send(new ObterUesComDrePorCodigoEModalidadeQuery(DRE_CODIGO_1,modalidades));
            retorno.ShouldNotBeNull();
            retorno.FirstOrDefault().DreId.ShouldBe(DRE_ID_1);
            retorno.FirstOrDefault().CodigoUe.ShouldBe(DRE_CODIGO_1);
        }
    }
}