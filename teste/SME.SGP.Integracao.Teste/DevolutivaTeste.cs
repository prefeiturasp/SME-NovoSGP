using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;
using Xunit.Extensions.Ordering;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class DevolutivaTeste
    {
        private readonly TestServerFixture fixture;

        public DevolutivaTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async void Deve_Obter_Devolutiva()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DE_C }));

            string id = "1";
            HttpResponseMessage result = await fixture._clientApi.GetAsync($"api/v1/devolutivas/{id}");

            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }

        [Fact]
        public void Deve_Inserir_Devolutiva()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DE_I }));

            InserirDevolutivaDto DevolutivaDto = new InserirDevolutivaDto()
            {
                TurmaCodigo = "123",
                CodigoComponenteCurricular = 1,
                PeriodoInicio = DateTime.Today.AddDays(-15),
                PeriodoFim = DateTime.Today.AddDays(15),
                Descricao = "Teste de Inclusão de Devolutivas... Teste de Inclusão de Devolutivas... Teste de Inclusão de Devolutivas... Teste de Inclusão de Devolutivas... Teste de Inclusão de Devolutivas... "
            };

            StringContent jsonParaPost = new StringContent(TransformarEmJson(DevolutivaDto), UnicodeEncoding.UTF8, "application/json");
            var postResult = fixture._clientApi.PostAsync("api/v1/devolutivas/", jsonParaPost).Result;

            Assert.True(fixture.ValidarStatusCodeComSucesso(postResult));
        }

        [Fact]
        public void Deve_Alterar_Devolutiva()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DE_A }));

            AlterarDevolutivaDto DevolutivaDto = new AlterarDevolutivaDto()
            {
                Id = 1,
                TurmaCodigo = "123",
                Descricao = "Teste de Alteração de Devolutivas... Teste de Alteração de Devolutivas... Teste de Alteração de Devolutivas... Teste de Alteração de Devolutivas... Teste de Alteração de Devolutivas... "
            };

            StringContent jsonParaPost = new StringContent(TransformarEmJson(DevolutivaDto), UnicodeEncoding.UTF8, "application/json");
            var postResult = fixture._clientApi.PutAsync("api/v1/devolutivas/1", jsonParaPost).Result;

            Assert.True(fixture.ValidarStatusCodeComSucesso(postResult));
        }

        [Fact]
        public void Deve_Excluir_Devolutiva()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DE_E }));

            var postResult = fixture._clientApi.DeleteAsync("api/v1/devolutivas/1").Result;

            Assert.True(fixture.ValidarStatusCodeComSucesso(postResult));
        }

        private string TransformarEmJson(object model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class DevolutivaTeste
    {
        private readonly TestServerFixture fixture;

        public DevolutivaTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }


        [Fact]
        public async void Deve_Obter_Sugestao_DataInicio()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DE_C }));

            string turmaCodigo = "1";
            long componenteCurricularId = 1105;
            HttpResponseMessage result = await fixture._clientApi.GetAsync($"api/v1/devolutivas/turmas/{turmaCodigo}/componentes-curriculares/{componenteCurricularId}/sugestao");

            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }

        [Fact]
        public async void Deve_Obter_Devolutiva_Por_Id()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DE_C }));

            long devolutivaId = 1;
            HttpResponseMessage result = await fixture._clientApi.GetAsync($"api/v1/devolutivas/{devolutivaId}");

            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }
    }
}
