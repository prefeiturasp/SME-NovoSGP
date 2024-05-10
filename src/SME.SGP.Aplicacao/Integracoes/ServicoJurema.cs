using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public class ServicoJurema : IServicoJurema
    {
        private readonly HttpClient httpClient;

        public ServicoJurema(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<ObjetivoAprendizagemResposta>> ObterListaObjetivosAprendizagem()
        {
            var resposta = await httpClient.GetAsync("v1/learning_objectives");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<ObjetivoAprendizagemResposta>>(json);
            }
            return null;
        }

    }
}