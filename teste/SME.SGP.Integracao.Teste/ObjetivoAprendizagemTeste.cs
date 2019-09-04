using Newtonsoft.Json;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using Xunit.Extensions.Ordering;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class ObjetivoAprendizagemTeste
    {
        private readonly TestServerFixture fixture;

        public ObjetivoAprendizagemTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact, Order(8)]
        public void Deve_Consultar_Objetivos_Aprendizagem()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();

            var filtros = new FiltroObjetivosAprendizagemDto();
            var jsonParaPost = new StringContent(TransformarEmJson(filtros), UnicodeEncoding.UTF8, "application/json");

            var getResult = fixture._clientApi.PostAsync("api/v1/objetivos-aprendizagem/", jsonParaPost).Result;

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