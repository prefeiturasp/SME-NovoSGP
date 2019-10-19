using Newtonsoft.Json;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SME.SGP.Integracao.Teste
{
    public class TesteBase
    {
        public static TestServerFixture ObtenhaCabecalhoAuthentication(TestServerFixture _fixture, Permissao[] permissoes)
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(permissoes));

            return _fixture;
        }

        public static HttpResponseMessage ExecutePostAsync(object ObjetoEnviar, TestServerFixture _fixture, string Url)
        {
            var jsonParaPost = new StringContent(JsonConvert.SerializeObject(ObjetoEnviar), Encoding.UTF8, "application/json");

            return _fixture._clientApi.PostAsync(Url, jsonParaPost).Result;
        }

        public static HttpResponseMessage ExecuteDeleteAsync(TestServerFixture _fixture, string Url)
        {
            return _fixture._clientApi.DeleteAsync(Url).Result;
        }

        public static HttpResponseMessage ExecuteGetAsync(TestServerFixture _fixture, string Url)
        {
            return _fixture._clientApi.GetAsync(Url).Result;
        }
    }
}
