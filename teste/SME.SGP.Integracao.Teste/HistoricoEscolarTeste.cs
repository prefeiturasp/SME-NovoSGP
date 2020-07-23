using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using SME.SGP.Infra.Dtos.Relatorios;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class HistoricoEscolarTeste
    {
        private readonly TestServerFixture fixture;

        public HistoricoEscolarTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact(DisplayName = "Retornar histórico escolar")]
        [Trait("Histórico escolar", "gerar")]
        public async void Gerar_Relatorio_Historico_Escolar()
        {
            // Arrange
            FiltroHistoricoEscolarDto filtro = new FiltroHistoricoEscolarDto();
            var jsonParaPost = new StringContent(JsonConvert.SerializeObject(filtro), Encoding.UTF8, "application/json");
            // Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { }));
            var result = await fixture._clientApi.PostAsync($"api/v1/historico-escolar/gerar", jsonParaPost);

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }
    }
}
