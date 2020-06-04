using SME.SGP.Infra;
using System;
using System.Net.Http.Headers;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class ConselhoClasseTeste
    {
        private readonly TestServerFixture fixture;
        private readonly long _id = 1;

        public ConselhoClasseTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact(DisplayName = "Retornar detalhamento de nota")]
        [Trait("Conselho de Classe", "Detalhamento de Nota")]
        public async void Retornar_Detalhamento_De_Nota()
        {
            // Arrange & Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { }));
            var result = await fixture._clientApi.GetAsync($"api/v1/conselhos-classe/detalhamento/{_id}");

            // Assert
            Assert.True(result.IsSuccessStatusCode);
        }
    }
}