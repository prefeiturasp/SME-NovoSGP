using MediatR;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosPorTurmaMapochoQueryHandler : IRequestHandler<ObterAlunosPorTurmaMapochoQuery, string[]>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterAlunosPorTurmaMapochoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<string[]> Handle(ObterAlunosPorTurmaMapochoQuery request, CancellationToken cancellationToken)
        {
            string[] alunos;

            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"turmas/{request.TurmaCodigo}/calculo-frequencia");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                alunos = JsonConvert.DeserializeObject<string[]>(json);
            }
            else alunos = new string[0];

            return alunos;
        }
    }
}
