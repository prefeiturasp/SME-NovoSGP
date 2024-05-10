using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
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

            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_TURMAS_CALCULO_FREQUENCIA, request.TurmaCodigo));
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
