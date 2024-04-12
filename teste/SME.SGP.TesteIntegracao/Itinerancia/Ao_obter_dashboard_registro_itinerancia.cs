using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Itinerancia.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Itinerancia
{
    public class Ao_obter_dashboard_registro_itinerancia : ItineranciaBase
    {
        public Ao_obter_dashboard_registro_itinerancia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Dashboard Registro de itinerância - Obter visitas paai")]
        public async Task Ao_obter_dashboard_itinerancia_visitas_paai()
        {
            await CriarDadosBase(new FiltroItineranciaDto() { AnoTurma = "5", ConsiderarAnoAnterior = false, Modalidade = Modalidade.Fundamental, Perfil = ObterPerfilCoordenadorCefai() });

            await InserirNaBase(new Dominio.Itinerancia()
            {
                AnoLetivo = DateTime.Now.Year,
                DataVisita = DateTimeExtension.HorarioBrasilia(),
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                Situacao = SituacaoItinerancia.Digitado,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new Dominio.Itinerancia()
            {
                AnoLetivo = DateTime.Now.Year,
                DataVisita = DateTimeExtension.HorarioBrasilia(),
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                Situacao = SituacaoItinerancia.Enviado,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });


            var useCase = ServiceProvider.GetService<IObterDashboardItineranciaVisitasPAAIsUseCase>();
            var filtro = new FiltroDashboardItineranciaDto()
            {
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                Mes = DateTimeExtension.HorarioBrasilia().Month
            };

            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.TotalRegistro.ShouldBe(2);
            retorno.DashboardItinerancias.Count().ShouldBe(1);
            retorno.DashboardItinerancias.First().Quantidade.ShouldBe(2);
        }
    }
}
