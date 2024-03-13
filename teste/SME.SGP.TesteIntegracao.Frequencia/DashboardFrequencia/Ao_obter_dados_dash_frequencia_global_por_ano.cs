using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia.DashboardFrequencia
{
    public  class Ao_obter_dados_dash_frequencia_global_por_ano : FrequenciaTesteBase
    {
        public Ao_obter_dados_dash_frequencia_global_por_ano(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);    
        }


        [Fact(DisplayName = "Deve retornar a informação do tipo 3 - Anual dos dados consolidados EJA")]
        public async Task Ao_consolidar_frequencia_turma_mensal_do_mes()
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().AddMonths(-1); 
            await CriarDreUePerfilComponenteCurricular();
            await CriarTipoCalendario(ModalidadeTipoCalendario.EJA);
            await CriarTurma(Modalidade.EJA, ANO_4, TURMA_CODIGO_1);

            await InserirNaBase(new ConsolidacaoFrequenciaTurma()
            {
                TurmaId = 1,
                QuantidadeAbaixoMinimoFrequencia = 0,
                QuantidadeAcimaMinimoFrequencia = 10,
                TipoConsolidacao = TipoConsolidadoFrequencia.Anual,
                PeriodoFim = null,
                PeriodoInicio = null,
                TotalAulas = 1,
                TotalFrequencias = 10
            });

            var useCase = ServiceProvider.GetService<IObterDashboardFrequenciaPorAnoUseCase>();

            var dadosConsolidados = await useCase.Executar(DateTime.Now.Year,1,1,Modalidade.EJA,0);

            dadosConsolidados.ShouldNotBeEmpty();
            dadosConsolidados.Count().ShouldBe(1);
            dadosConsolidados.Any(d => d.Quantidade == 10).ShouldBeTrue();
        }
    }
}
