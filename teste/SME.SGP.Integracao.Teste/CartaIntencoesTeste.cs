using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class CartaIntencoesTeste
    {
        private readonly TestServerFixture fixture;

        public CartaIntencoesTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async Task Deve_Obter_Carta_Intencoes()
        {
            // Arrange 
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.CI_C }));

            var turmaCodigo = "123";
            var componenteCurricularId = 512;

            // Act
            var result = await fixture._clientApi.GetAsync($"api/v1/carta-intencoes/turmas/{turmaCodigo}/componente-curricular/{componenteCurricularId}");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }
    }
}
