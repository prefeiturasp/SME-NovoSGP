using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.ConsolidacaoMatriculaTurma.ObterMatriculasConsolidacaoAnosAnteriores
{
    public class ObterMatriculasConsolidacaoAnosAnterioresQueryHandler : IRequestHandler<ObterMatriculasConsolidacaoAnosAnterioresQuery, IEnumerable<ConsolidacaoMatriculaTurmaDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterMatriculasConsolidacaoAnosAnterioresQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<ConsolidacaoMatriculaTurmaDto>> Handle(ObterMatriculasConsolidacaoAnosAnterioresQuery request, CancellationToken cancellationToken)
        {
            var matriculasConsolidadas = Enumerable.Empty<ConsolidacaoMatriculaTurmaDto>();

            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.GetAsync(ServicosEolConstants.URL_MATRICULAS_ANOS_ANTERIORES + $"?AnoLetivo={request.AnoLetivo}&ueCodigo={request.UeCodigo}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                matriculasConsolidadas = JsonConvert.DeserializeObject<List<ConsolidacaoMatriculaTurmaDto>>(json);
            }
            return matriculasConsolidadas;
        }
    }
}
