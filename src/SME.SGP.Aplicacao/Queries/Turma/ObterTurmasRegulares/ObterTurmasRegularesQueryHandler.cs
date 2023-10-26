using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasRegularesQueryHandler : IRequestHandler<ObterTurmasRegularesQuery, IEnumerable<string>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterTurmasRegularesQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<string>> Handle(ObterTurmasRegularesQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var parametros = JsonConvert.SerializeObject(request.CodigosTurmas);
            
            var resposta = await httpClient.PostAsync(ServicosEolConstants.URL_TURMAS_REGULARES, new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode || resposta.StatusCode == HttpStatusCode.NoContent)
                return Enumerable.Empty<string>();

            var json = await resposta.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<string>>(json);
        }
    }
}
