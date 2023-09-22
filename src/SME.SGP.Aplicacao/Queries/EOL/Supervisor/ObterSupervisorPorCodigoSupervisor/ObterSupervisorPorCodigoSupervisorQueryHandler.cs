using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSupervisorPorCodigoSupervisorQueryHandler : IRequestHandler<ObterSupervisorPorCodigoSupervisorQuery, IEnumerable<SupervisoresRetornoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterSupervisorPorCodigoSupervisorQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<IEnumerable<SupervisoresRetornoDto>> Handle(ObterSupervisorPorCodigoSupervisorQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.PostAsync(ServicosEolConstants.URL_FUNCIONARIOS_SUPERVISORES, new StringContent(JsonConvert.SerializeObject(request.CodigoSupervisores), Encoding.UTF8, "application/json-patch+json"));

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<SupervisoresRetornoDto>>(json);
            }

            return Enumerable.Empty<SupervisoresRetornoDto>();
        }
    }
}
