using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Text;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class AutenticacaoTeste
    {
        private readonly TestServerFixture fixture;

        public AutenticacaoTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public void Deve_Retornar_Nao_Autorizado()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();

            var autenticacaoDto = new AutenticacaoDto() { Login = "testea", Senha = "senha123" };

            var jsonParaPost = new StringContent(JsonConvert.SerializeObject(autenticacaoDto), UnicodeEncoding.UTF8, "application/json");

            var postResult = fixture._clientApi.PostAsync($"api/v1/autenticacao", jsonParaPost).Result;
            Assert.True(!postResult.IsSuccessStatusCode);
            Assert.True(postResult.StatusCode == System.Net.HttpStatusCode.Unauthorized);
        }
    }
}