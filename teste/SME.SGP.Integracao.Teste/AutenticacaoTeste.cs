using Newtonsoft.Json;
using SME.SGP.Dto;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
            Assert.True(postResult.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Fact]
        public void Deve_Retornar_Senha_Incorreta_Formatacao()
        {
            var primeiroAcessoDto = new PrimeiroAcessoDto { ConfirmarSenha = "A1234567", NovaSenha = "A1234567", Usuario = "7777710" };

            var resultado = RequisicaoPrimeiroAcesso(primeiroAcessoDto).Result;

            Assert.False(resultado.IsSuccessStatusCode);
            //Assert.Equal(HttpStatusCode.Unauthorized, resultado.StatusCode);
        }

        [Fact]
        public void Deve_Retornar_Senha_Incorreta_Tamanho_Pequeno()
        {
            var primeiroAcessoDto = new PrimeiroAcessoDto { ConfirmarSenha = "Aa1", NovaSenha = "Aa1", Usuario = "7777710" };

            var resultado = RequisicaoPrimeiroAcesso(primeiroAcessoDto).Result;

            Assert.False(resultado.IsSuccessStatusCode);
            //Assert.Equal(HttpStatusCode.Unauthorized, resultado.StatusCode);
        }

        [Fact]
        public void Deve_Retornar_Senha_Incorreta_Tamanho_Grande()
        {
            var primeiroAcessoDto = new PrimeiroAcessoDto { ConfirmarSenha = "Aa1234567891011", NovaSenha = "Aa1234567891011", Usuario = "7777710" };

            var resultado = RequisicaoPrimeiroAcesso(primeiroAcessoDto).Result;

            Assert.False(resultado.IsSuccessStatusCode);
            //Assert.Equal(HttpStatusCode.Unauthorized, resultado.StatusCode);
        }

        [Fact]
        public void Deve_Retornar_Senha_Incorreta_Espaco()
        {
            var primeiroAcessoDto = new PrimeiroAcessoDto { ConfirmarSenha = "Aa 1234567", NovaSenha = "Aa 1234567", Usuario = "7777710" };

            var resultado = RequisicaoPrimeiroAcesso(primeiroAcessoDto).Result;

            Assert.False(resultado.IsSuccessStatusCode);
            //Assert.Equal(HttpStatusCode.Unauthorized, resultado.StatusCode);
        }

        [Fact]
        public void Deve_Retornar_Usuario_Nao_Encontrado()
        {
            var primeiroAcessoDto = new PrimeiroAcessoDto { ConfirmarSenha = "Aa1234567", NovaSenha = "Aa1234567", Usuario = "123123123123" };

            var resultado = RequisicaoPrimeiroAcesso(primeiroAcessoDto).Result;

            Assert.False(resultado.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, resultado.StatusCode);
        }


        private async Task<HttpResponseMessage> RequisicaoPrimeiroAcesso(PrimeiroAcessoDto primeiroAcessoDto)
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();                      

            var jsonParaPost = new StringContent(JsonConvert.SerializeObject(primeiroAcessoDto), UnicodeEncoding.UTF8, "application/json");

            var postResult = await fixture._clientApi.PostAsync($"api/v1/autenticacao/PrimeiroAcesso", jsonParaPost);

            return postResult;
        }
    }
}