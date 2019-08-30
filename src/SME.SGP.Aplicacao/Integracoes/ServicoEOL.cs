using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public class ServicoEOL : IServicoEOL
    {
        private readonly HttpClient httpClient;

        public ServicoEOL(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPorProfessorETurma(long codigoTurma, string rfProfessor)
        {
            var resposta = await httpClient.GetAsync($"professores/{rfProfessor}/turmas/{codigoTurma}/disciplinas");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<DisciplinaResposta>>(json);
            }
            return null;
        }
    }
}