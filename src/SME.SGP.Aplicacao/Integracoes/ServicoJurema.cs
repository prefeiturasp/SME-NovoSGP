using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using System;
using System.Collections.Generic;
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

        public IEnumerable<ObjetivoAprendizagemResposta> ObterListaObjetivosAprendizagem()
        {
            var teste = httpClient.GetAsync("v1/learning_objectives");
            var json = teste.Result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<IEnumerable<ObjetivoAprendizagemResposta>>(json);
        }
    }
}