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
    public class RelatorioPendenciasFechamentoTeste
    {
        private readonly TestServerFixture fixture;

        public RelatorioPendenciasFechamentoTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        //[Fact(DisplayName = "Relatório de Pendências no Fechamento")]
        //[Trait("Pendências no Fechamento", "gerar")]
        //public async void Deve_Realizar_Chamada_Para_Gerar_Relatorio()
        //{
        //    // Arrange
        //    FiltroRelatorioPendenciasFechamentoDto filtro = new FiltroRelatorioPendenciasFechamentoDto();
        //    var jsonParaPost = new StringContent(JsonConvert.SerializeObject(filtro), Encoding.UTF8, "application/json");
        //    // & Act
        //    fixture._clientApi.DefaultRequestHeaders.Clear();
        //    fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { }));
        //    var result = await fixture._clientApi.PostAsync($"api/v1/relatorio/pendencias-fechamento/gerar", jsonParaPost);

        //    // Assert
        //    Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        //}
    }
}
