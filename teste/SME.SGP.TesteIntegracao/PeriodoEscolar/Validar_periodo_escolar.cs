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
            retorno.TipoCalendarioId.ShouldBe(TIPO_CALENDARIO_7);
            retorno.PeriodoInicio.ShouldBe(DATA_25_07_INICIO_BIMESTRE_3);
        }
    }
}