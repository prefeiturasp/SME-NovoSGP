using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterMatriculasAlunoPorCodigoEAnoQueryHandler : IRequestHandler<ObterMatriculasAlunoPorCodigoEAnoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterMatriculasAlunoPorCodigoEAnoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterMatriculasAlunoPorCodigoEAnoQuery request, CancellationToken cancellationToken)
        {
            var alunos = Enumerable.Empty<AlunoPorTurmaResposta>();

            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"alunos/{request.CodigoAluno}/turmas/anosLetivos/{request.AnoLetivo}/historico/{request.Historico}/filtrar-situacao/{request.FiltrarSituacao}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                alunos = JsonConvert.DeserializeObject<List<AlunoPorTurmaResposta>>(json);
            }

            return alunos;
        }
    }
}
