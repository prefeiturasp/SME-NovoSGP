using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class UsuariosTeste
    {
        private readonly TestServerFixture fixture;

        public UsuariosTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async void Deve_Alterar_Email_Usuario()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();

            fixture._clientApi.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { }));

            var alterarEmailDto = new AlterarEmailDto
            {
                NovoEmail = "novo@gmail.com"
            };

            var jsonParaPut = new StringContent(JsonConvert.SerializeObject(alterarEmailDto), Encoding.UTF8, "application/json");

            var putResult = await fixture._clientApi.PutAsync($"api/v1/usuarios/autenticado/email", jsonParaPut);

            Assert.True(putResult.IsSuccessStatusCode);
        }
    }
}