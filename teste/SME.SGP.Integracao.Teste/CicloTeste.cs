using SME.SGP.Infra;
using System;
using System.Net.Http.Headers;
using System.Text;
using Xunit;
using Newtonsoft.Json;
using System.Net.Http;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class CicloTeste
    {
        private readonly TestServerFixture fixture;

        public CicloTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact(DisplayName = "Retornar os ciclos")]
        [Trait("Ciclos", "Retornar os ciclos")]
        public async void Ciclos_Retornar_Ciclos()
        {
            // Arrange            
            List<string> anos = new List<string>();
            for (int ano = 1; ano <= 9; ano++) anos.Add(ano.ToString());
            FiltroCicloDto filtroCiclo = new FiltroCicloDto { Anos = anos, AnoSelecionado = "1", Modalidade = 5 };

            // Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            var jsonParaPost = new StringContent(JsonConvert.SerializeObject(filtroCiclo), Encoding.UTF8, "application/json");
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { }));
            var result = await fixture._clientApi.PostAsync("api/v1/ciclos/filtro", jsonParaPost);

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }
    }
}