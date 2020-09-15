using SME.SGP.Infra.Json;
using SME.SGP.Api.Controllers;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class MenuTeste
    {
        private readonly TestServerFixture fixture;

        public MenuTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async void Deve_Retornar_Menu()
        {
   
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.N_C, Permissao.PA_I }));

            var result = await fixture._clientApi.GetAsync("api/v1/menus/");

            Assert.True(result.IsSuccessStatusCode);

            var menu = SgpJsonSerializer.Deserialize<List<MenuRetornoDto>>(await result.Content.ReadAsStringAsync());

            Assert.True(menu.Count() > 0);
        }
    }
}