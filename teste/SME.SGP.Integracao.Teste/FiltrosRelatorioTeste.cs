using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Net.Http.Headers;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class FiltrosRelatorioTeste
    {
        private readonly TestServerFixture fixture;

        public FiltrosRelatorioTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact(DisplayName = "Obter Dres por abrangência com opção todas")]
        [Trait("FiltrosRelatorio", "Obter Dres por abrangência com opção todas")]
        public async void ObterDresPorAbrangenciaComOpcaoTodas()
        {
            // Arrange            
            // Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { }));
            
            var result = await fixture._clientApi.GetAsync($"api/v1/relatorios/filtros/dres");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }

        [Fact(DisplayName = "Obter Ues por abrangência com opção todas")]
        [Trait("FiltrosRelatorio", "Obter Ues por abrangência com opção todas")]
        public async void ObterUesPorAbrangenciaComOpcaoTodas()
        {
            // Arrange            
            // Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { }));

            var codigoDre = "108800";
            var result = await fixture._clientApi.GetAsync($"api/v1/relatorios/filtros/dres/{codigoDre}/ues");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }


        [Fact(DisplayName = "Obter modalidades por Ue")]
        [Trait("FiltrosRelatorio", "Obter modalidades por Ue")]
        public async void ObterModalidadesPorUe()
        {
            // Arrange            
            // Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { }));

            var codigoUe = "094765";
            var result = await fixture._clientApi.GetAsync($"api/v1/relatorios/filtros/ues/{codigoUe}/modalidades");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }


        [Fact(DisplayName = "Obter anos escolares por modalidades Ue")]
        [Trait("FiltrosRelatorio", "Obter modalidades por Ue")]
        public async void ObterAnosEscolaresPorModalidadesUe()
        {
            // Arrange            
            // Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { }));

            var codigoUe = "094765";
            var modalidade = (int)Modalidade.Fundamental;
            var result = await fixture._clientApi.GetAsync($"api/v1/relatorios/filtros/ues/{codigoUe}/modalidades/{modalidade}/anos-escolares");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }
    }
}
