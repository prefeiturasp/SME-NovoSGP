using Newtonsoft.Json;
using SME.SGP.Dto;
using System.Collections.Generic;
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

        public async Task<IEnumerable<TurmaDto>> ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(string rfProfessor, long codigoEscola, int anoLetivo)
        {
            var resposta = await httpClient.GetAsync($"professores/{rfProfessor}/escolas/{codigoEscola}/turmas/anos_letivos/{anoLetivo}");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<TurmaDto>>(json);
            }
            return null;
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