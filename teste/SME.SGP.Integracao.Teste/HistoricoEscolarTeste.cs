using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class HistoricoEscolarTeste
    {
        private readonly TestServerFixture fixture;
        private readonly long _id = 1;



        public HistoricoEscolarTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        //[Fact(DisplayName = "Retornar histórico escolar")]
        //[Trait("Histórico escolar", "gerar")]
        //public async void Gerar_Relatorio_Historico_Escolar()
        //{
        //    // Arrange & Act
        //    fixture._clientApi.DefaultRequestHeaders.Clear();
        //    fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] {}));
        //    var result = await fixture._clientApi.GetAsync($"api/v1/historico-escolar/gerar");

        //    // Assert
        //    Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        //}
    }
}
