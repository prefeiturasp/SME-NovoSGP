using System;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class VersaoTeste
    {
        private readonly TestServerFixture fixture;

        public VersaoTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact(DisplayName = "Retornar a última versão da release")]
        [Trait("Versionamento", "Última versão da release")]
        public async void Versionamento_Retornar_Ultima_Versao()
        {
            // Arrange & Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            var result = await fixture._clientApi.GetAsync("api/v1/versoes/");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }
    }
}