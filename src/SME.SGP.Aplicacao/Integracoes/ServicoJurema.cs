using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;

namespace SME.SGP.Aplicacao.Integracoes
{
    public class ServicoJurema
    {
        private readonly HttpClient httpClient;

        public ServicoJurema(HttpClient httpClient, IConfiguration configuration)
        {
            httpClient.BaseAddress = new Uri(configuration.GetSection("UrlApiJurema").Value);
            httpClient.DefaultRequestHeaders.Add("Accept",
            "application/json");
            this.httpClient = httpClient;
        }

        public void ObterListaObjetivosAprendizagem()
        {
            var teste = httpClient.GetAsync("v1/learning_objectives");
            var resultado = teste.Result.Content.ReadAsStringAsync();
        }
    }
}