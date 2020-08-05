using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class DiarioBordoTeste
    {
        private readonly TestServerFixture fixture;

        public DiarioBordoTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact(DisplayName = "Obter Diário de Bordo")]
        [Trait("DiarioBordo", "Obter Diário de Bordo")]
        public async void Obter()
        {
            // Arrange
            // Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DDB_C }));

            var id = "31886";
            var result = await fixture._clientApi.GetAsync($"api/v1/diario-bordo/{id}");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }
    }
}
