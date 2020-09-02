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

        [Fact]
        public async void Deve_Obter_Diario_De_Bordo()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DDB_C }));

            string id = "1";
            HttpResponseMessage result = await fixture._clientApi.GetAsync($"api/v1/diarios-bordo/{id}");

            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }

        [Fact]
        public void Deve_Inserir_Diario_De_Bordo()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DDB_I }));

            InserirDiarioBordoDto diarioBordoDto = new InserirDiarioBordoDto()
            {
                AulaId = 1,
                Planejamento = "Teste de Inclusão de Diario de bordo... Teste de Inclusão de Diario de bordo... Teste de Inclusão de Diario de bordo... Teste de Inclusão de Diario de bordo... Teste de Inclusão de Diario de bordo... "
            };

            StringContent jsonParaPost = new StringContent(TransformarEmJson(diarioBordoDto), UnicodeEncoding.UTF8, "application/json");
            var postResult = fixture._clientApi.PostAsync("api/v1/diarios-bordo/", jsonParaPost).Result;

            Assert.True(fixture.ValidarStatusCodeComSucesso(postResult));
        }

        [Fact]
        public void Deve_Alterar_Diario_De_Bordo()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DDB_A }));

            AlterarDiarioBordoDto diarioBordoDto = new AlterarDiarioBordoDto()
            {
                Id = 1,
                AulaId = 1,
                Planejamento = "Teste de Inclusão de Diario de bordo... Teste de Inclusão de Diario de bordo... Teste de Inclusão de Diario de bordo... Teste de Inclusão de Diario de bordo... Teste de Inclusão de Diario de bordo... "
            };

            StringContent jsonParaPut = new StringContent(TransformarEmJson(diarioBordoDto), UnicodeEncoding.UTF8, "application/json");
            var putResult = fixture._clientApi.PutAsync($"api/v1/diarios-bordo/", jsonParaPut).Result;

            Assert.True(fixture.ValidarStatusCodeComSucesso(putResult));
        }

        [Fact]
        public async void Deve_Obter_Diarios_De_Bordo_Por_Intervalo()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { })); // TODO ajustar permissionamento

            string turmaCodigo = "1";
            long componenteCurricularId = 1105;
            var dataInicio = "2020-08-01";
            var dataFim = "2020-08-31";

            // TODO resolver problema na execução da query via teste
            //HttpResponseMessage result = await fixture._clientApi.GetAsync($"api/v1/diarios-bordo/turmas/{turmaCodigo}/componentes-curriculares/{componenteCurricularId}/inicio/{dataInicio}/fim/{dataFim}?NumeroPagina=1&&NumeroRegistros=4");

            //Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }

        [Fact]
        public void Deve_Adicionar_Observacao_Diario_De_Bordo()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DDB_C }));

            var dto = new ObservacaoDiarioBordoDto()
            {
                Observacao="Teste de Observação"
            };

            StringContent jsonParaPost = new StringContent(TransformarEmJson(dto), UnicodeEncoding.UTF8, "application/json");
            var postResult = fixture._clientApi.PostAsync("api/v1/diarios-bordo/1/observacoes", jsonParaPost).Result;

            Assert.True(fixture.ValidarStatusCodeComSucesso(postResult));
        }

        [Fact]
        public void Deve_Alterar_Observacao_Diario_De_Bordo()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DDB_C }));

            var dto = new ObservacaoDiarioBordoDto()
            {
                Observacao = "Teste de Observação"
            };

            StringContent jsonParaPost = new StringContent(TransformarEmJson(dto), UnicodeEncoding.UTF8, "application/json");
            var postResult = fixture._clientApi.PutAsync("api/v1/diarios-bordo/observacoes/1", jsonParaPost).Result;

            Assert.True(fixture.ValidarStatusCodeComSucesso(postResult));
        }

        [Fact]
        public void Deve_Excluir_Observacao_Diario_De_Bordo()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DDB_C }));

            var postResult = fixture._clientApi.DeleteAsync("api/v1/diarios-bordo/observacoes/1").Result;

            Assert.True(fixture.ValidarStatusCodeComSucesso(postResult));
        }

        [Fact]
        public void Deve_Listar_Observacao_Diario_De_Bordo()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DDB_C }));

            var postResult = fixture._clientApi.GetAsync("api/v1/diarios-bordo/1/observacoes").Result;

            Assert.True(fixture.ValidarStatusCodeComSucesso(postResult));
        }

        private string TransformarEmJson(object model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}
