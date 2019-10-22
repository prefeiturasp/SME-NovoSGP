using Newtonsoft.Json;
using SME.SGP.Dominio;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SME.SGP.Integracao.Teste
{
    public static class TesteBase
    {
        public static HttpResponseMessage ExecuteDeleteAsync(TestServerFixture _fixture, string Url)
        {
            return _fixture._clientApi.DeleteAsync(Url).Result;
        }

        public static HttpResponseMessage ExecuteDeleteAsync(TestServerFixture _fixture, string Url, object ObjetoEnviar)
        {
            var jsonParaPost = new StringContent(JsonConvert.SerializeObject(ObjetoEnviar), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(_fixture._clientApi.BaseAddress.AbsoluteUri + Url),
                Content = jsonParaPost
            };

            return _fixture._clientApi.SendAsync(request).Result;
        }

        public static HttpResponseMessage ExecuteGetAsync(TestServerFixture _fixture, string Url)
        {
            return _fixture._clientApi.GetAsync(Url).Result;
        }

        public static HttpResponseMessage ExecutePostAsync(TestServerFixture _fixture, string Url, object ObjetoEnviar)
        {
            var jsonParaPost = new StringContent(JsonConvert.SerializeObject(ObjetoEnviar), Encoding.UTF8, "application/json");

            return _fixture._clientApi.PostAsync(Url, jsonParaPost).Result;
        }

        public static TestServerFixture ObtenhaCabecalhoAuthentication(TestServerFixture _fixture, Permissao[] permissoes)
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(permissoes));

            return _fixture;
        }
    }
}