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
    public class RelatorioParecerConclusivoTeste
    {
        private readonly TestServerFixture fixture;

        public RelatorioParecerConclusivoTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact(DisplayName = "Relatório de Parecer Conclusivo")]
        [Trait("Relatório de Parecer Conclusivo", "obter ciclos")]
        public async void Deve_Retornar_Ciclos_Por_Modalidade()
        {
            // Arrange
            var filtro = new FiltroCicloPorModalidadeECodigoUeDto(3, "092789");
            var jsonParaPost = new StringContent(JsonConvert.SerializeObject(filtro), Encoding.UTF8, "application/json");
            // & Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { }));
            var result = await fixture._clientApi.PostAsync($"api/v1/relatorios/pareceres-conclusivos/ciclos", jsonParaPost);

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }
    }
}
