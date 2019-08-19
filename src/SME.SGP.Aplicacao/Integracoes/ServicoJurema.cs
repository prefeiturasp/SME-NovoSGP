using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using System.Collections.Generic;
using System.Net.Http;

namespace SME.SGP.Aplicacao.Integracoes
{
    public class ServicoJurema : IServicoJurema
    {
        private readonly HttpClient httpClient;

        public ServicoJurema(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public IEnumerable<ObjetivoAprendizagemResposta> ObterListaObjetivosAprendizagem()
        {
            var resposta = httpClient.GetAsync("v1/learning_objectives").Result;
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<ObjetivoAprendizagemResposta>>(json);
            }
            return null;
        }
    }
}