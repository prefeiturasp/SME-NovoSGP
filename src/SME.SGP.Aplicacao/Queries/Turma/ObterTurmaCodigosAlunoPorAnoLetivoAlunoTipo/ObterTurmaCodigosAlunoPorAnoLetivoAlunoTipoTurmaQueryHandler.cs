using MediatR;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQueryHandler : IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<string[]> Handle(ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery request, CancellationToken cancellationToken)
        {
            var tiposTurma = String.Join("&tiposTurma=", Enumerable.Cast<int>(request.TiposTurmas));
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"turmas/anos-letivos/{request.AnoLetivo}/alunos/{request.CodigoAluno}/regulares?tiposTurma={tiposTurma}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<string[]>(json);
            }
            return new string[] { };
        }
    }
}
