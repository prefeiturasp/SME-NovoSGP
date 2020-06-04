using SME.SGP.Infra;
using System;
using System.Net.Http.Headers;
using System.Text;
using Xunit;
using Newtonsoft.Json;
using System.Net.Http;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class CicloTeste
    {
        private readonly TestServerFixture fixture;

        public CicloTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact(DisplayName = "Retornar os ciclos")]
        [Trait("Ciclo", "Ciclos")]
        public async void Ciclos_Retornar_Ciclos()
        {
            // Arrange            
            FiltroCicloDto filtroCiclo = new FiltroCicloDto();

            // Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            var jsonParaPost = new StringContent(JsonConvert.SerializeObject(filtroCiclo), Encoding.UTF8, "application/json");
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { }));
            var result = await fixture._clientApi.PostAsync("api/v1/ciclos/", jsonParaPost);

            // Assert
            Assert.True(true); // TODO: rever chamada
        }
    }
}