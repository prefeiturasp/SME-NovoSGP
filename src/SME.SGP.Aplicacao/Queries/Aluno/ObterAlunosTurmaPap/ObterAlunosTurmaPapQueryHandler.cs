using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Aluno.ObterAlunosTurmaPap
{
    public class ObterAlunosTurmaPapQueryHandler : IRequestHandler<ObterAlunosTurmaPapQuery, IEnumerable<DadosMatriculaAlunoTipoPapDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterAlunosTurmaPapQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<DadosMatriculaAlunoTipoPapDto>> Handle(ObterAlunosTurmaPapQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var url = request.AnoLetivo == null ?
                      ServicosEolConstants.URL_ALUNOS_PAP_ANO_CORRENTE :
                      string.Format(ServicosEolConstants.URL_ALUNOS_TURMAS_PAP_POR_ANO, request.AnoLetivo);

            var response = await httpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Não foi possível obter os alunos PAP. Erro: {response.StatusCode} - {response.ReasonPhrase}");

            var alunosPap = await response.Content.ReadAsStringAsync(cancellationToken);
            return !string.IsNullOrEmpty(alunosPap) ?
                   System.Text.Json.JsonSerializer.Deserialize<IEnumerable<DadosMatriculaAlunoTipoPapDto>>(alunosPap) :
                   Enumerable.Empty<DadosMatriculaAlunoTipoPapDto>();
        }
    }
}
