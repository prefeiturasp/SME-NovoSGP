using Xunit;

namespace SME.SGP.Integracao.Teste
{

    [Collection("Testserver collection")]
    public class RelatorioPAPTeste
    {
        private TestServerFixture fixture;
        public RelatorioPAPTeste(TestServerFixture fixture)
        {
            this.fixture = fixture;
        }


        [Fact(DisplayName = "Obter anos letivos para PAP")]
        [Trait("Relatórios PAP", "Anos Letivos")]
        public async void Versionamento_Retornar_Ultima_Versao()
        {
            // Arrange & Act            
            var result = await fixture._clientApi.GetAsync("api/v1/recuperacao-paralela/anos-letivos");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }

    }
}
