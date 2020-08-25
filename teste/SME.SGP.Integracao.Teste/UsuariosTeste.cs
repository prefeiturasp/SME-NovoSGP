using SME.SGP.Infra;
using System;
using System.Net.Http.Headers;
using Xunit;
using Xunit.Extensions.Ordering;

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

        //TODO: CHAVE INTEGRAÇÃO API EOL
        //[Fact, Order(1)]
        //public async void Deve_Retornar_Os_Dados_Do_Usuário()
        //{
        //    fixture._clientApi.DefaultRequestHeaders.Clear();
        //    fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.M_C }, "7777710", "7777710"));

        //    var result = await fixture._clientApi.GetAsync("api/v1/usuarios/meus-dados");

        //    Assert.True(result.IsSuccessStatusCode);
        //}
    }
}