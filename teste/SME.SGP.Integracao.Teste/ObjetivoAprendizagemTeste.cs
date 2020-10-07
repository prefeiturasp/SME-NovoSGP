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
    public class ObjetivoAprendizagemTeste
    {
        private readonly TestServerFixture _fixture;

        public ObjetivoAprendizagemTeste(TestServerFixture fixture)
        {
            this._fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async void Deve_Listar_Todos_Objetivos()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();
            _fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.PA_C }));

            long ano = 1;
            long componenteCurricularId = 1105;
            HttpResponseMessage result = await _fixture._clientApi.GetAsync($"api/v1/objetivos-aprendizagem/{ano}/{componenteCurricularId}");

            Assert.True(_fixture.ValidarStatusCodeComSucesso(result));
        }

        [Fact, Order(8)]
        public void Deve_Consultar_Objetivos_Aprendizagem()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.PA_C, Permissao.PA_A, Permissao.PA_I, Permissao.PA_E }));

            var filtros = new FiltroObjetivosAprendizagemDto();
            filtros.ComponentesCurricularesIds.Add(139);
            filtros.Ano = "3";

            var jsonParaPost = new StringContent(TransformarEmJson(filtros), UnicodeEncoding.UTF8, "application/json");

            var getResult = _fixture._clientApi.PostAsync("api/v1/objetivos-aprendizagem/", jsonParaPost).Result;

            Assert.True(getResult.IsSuccessStatusCode);
            var disciplinas = JsonConvert.DeserializeObject<IEnumerable<ObjetivoAprendizagemDto>>(getResult.Content.ReadAsStringAsync().Result);
            Assert.True(disciplinas != null);
        }

        private string TransformarEmJson(object model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}