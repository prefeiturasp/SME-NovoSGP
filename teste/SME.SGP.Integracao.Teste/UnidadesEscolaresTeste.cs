using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class UnidadesEscolaresTeste
    {
        private readonly TestServerFixture fixture;

        public UnidadesEscolaresTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async void Deve_Retornar_Funcionarios()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();

            fixture._clientApi.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { }));

            var getResult = await fixture._clientApi.GetAsync($"api/v1/unidades-escolares/{000892}/functionarios");

            if (getResult.IsSuccessStatusCode)
            {
                var funcionarios = JsonConvert.DeserializeObject<List<UsuarioEolRetornoDto>>(await getResult.Content.ReadAsStringAsync());

                Assert.True(funcionarios.Count > 0);
            }
        }
    }
}