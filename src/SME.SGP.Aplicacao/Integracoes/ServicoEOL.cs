using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using System.Collections.Generic;
using System.Net.Http;

namespace SME.SGP.Aplicacao.Integracoes
{
    public class ServicoEOL : IServicoEOL
    {
        private readonly HttpClient httpClient;

        public ServicoEOL(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public IEnumerable<ProfessorTurmaReposta> ObterListaTurmasPorProfessor(string codigoRf)
        {
            var resposta = httpClient.GetAsync($"professores/{codigoRf}/turmas").Result;
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<ProfessorTurmaReposta>>(json);
            }
            return null;
        }
    }
}