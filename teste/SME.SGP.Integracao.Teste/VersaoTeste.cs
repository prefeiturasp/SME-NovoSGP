using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [Trait("Versionamento", "Github api")]
        public async void Versionamento_Retornar_Ultima_Versao()
        {
            // Arrange & Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            var result = await fixture._clientApi.GetAsync("api/v1/versoes/");
            var versao = await result.Content.ReadAsStringAsync();

            // Assert
            Assert.True(result.IsSuccessStatusCode);
            //Assert.StartsWith("Versão: ", versao);
        }
    }
}