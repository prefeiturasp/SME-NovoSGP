using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosDreOuUePorPerfisQueryHandler : IRequestHandler<ObterFuncionariosDreOuUePorPerfisQuery, IEnumerable<FuncionarioUnidadeDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterFuncionariosDreOuUePorPerfisQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<FuncionarioUnidadeDto>> Handle(ObterFuncionariosDreOuUePorPerfisQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var parametros = JsonConvert.SerializeObject(request.Perfis);

            var resposta = await httpClient.PostAsync($"/api/" + string.Format(ServicosEolConstants.URL_FUNCIONARIOS_UNIDADE, request.CodigoDreUe), new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"), cancellationToken);

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
                return JsonConvert.DeserializeObject<IEnumerable<FuncionarioUnidadeDto>>(json);
            }

            return Enumerable.Empty<FuncionarioUnidadeDto>();
        }
    }
}
