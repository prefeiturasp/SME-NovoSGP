using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;
using Xunit.Extensions.Ordering;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class DiarioBordoTeste
    {
        private readonly TestServerFixture fixture;

        public DiarioBordoTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact, Order(1)]
        public async void Deve_Obter_Diario_De_Bordo()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DDB_C }));

            string id = "31886";
            HttpResponseMessage result = await fixture._clientApi.GetAsync($"api/v1/diario-bordo/{id}");

            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }

        [Fact, Order(1)]
        public void Deve_Inserir_Diario_De_Bordo()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();

            fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DDB_I }));

            InserirDiarioBordoDto diarioBordoDto = new InserirDiarioBordoDto();

            StringContent jsonParaPost = new StringContent(TransformarEmJson(diarioBordoDto), UnicodeEncoding.UTF8, "application/json");

            HttpResponseMessage postResult = fixture._clientApi.PostAsync("api/v1/diario-bordo/", jsonParaPost).Result;
            string result = postResult.Content.ReadAsStringAsync().Result;

            Assert.True(postResult.IsSuccessStatusCode, result);
        }

        [Fact, Order(2)]
        public void Deve_Alterar_Diario_De_Bordo()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();

            fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DDB_I }));

            AlterarDiarioBordoDto diarioBordoDto = new AlterarDiarioBordoDto();

            StringContent jsonParaPut = new StringContent(TransformarEmJson(diarioBordoDto), UnicodeEncoding.UTF8, "application/json");

            HttpResponseMessage postResult = fixture._clientApi.PutAsync("api/v1/diario-bordo/", jsonParaPut).Result;
            string result = postResult.Content.ReadAsStringAsync().Result;

            Assert.True(postResult.IsSuccessStatusCode, result);
        }

        private string TransformarEmJson(object model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}
