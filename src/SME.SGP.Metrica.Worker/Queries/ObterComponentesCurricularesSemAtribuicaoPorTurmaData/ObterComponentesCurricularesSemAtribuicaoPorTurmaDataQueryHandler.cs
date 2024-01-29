using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.Queries
{
    public class ObterComponentesCurricularesSemAtribuicaoPorTurmaDataQueryHandler : IRequestHandler<ObterComponentesCurricularesSemAtribuicaoPorTurmaDataQuery, IEnumerable<string>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterComponentesCurricularesSemAtribuicaoPorTurmaDataQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<string>> Handle(ObterComponentesCurricularesSemAtribuicaoPorTurmaDataQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_COMPONENTES_CURRICULARES_TURMA_SEM_ATRIBUICAO_AULA, request.CodigoTurma, request.Data.Ticks));
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<string>>(json);
            }

            return Enumerable.Empty<string>();
        }
    }
}
