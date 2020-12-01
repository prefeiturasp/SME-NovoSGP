using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosSmePorPerfisQueryHandler : IRequestHandler<ObterFuncionariosSmePorPerfisQuery, IEnumerable<string>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterFuncionariosSmePorPerfisQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<string>> Handle(ObterFuncionariosSmePorPerfisQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");

            var parametros = JsonConvert.SerializeObject(request.Perfis);

            var resposta = await httpClient.PostAsync("admins/sme", new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<string>>(json);
            }
            else throw new NegocioException("Não foi possível obter os funcionários da API EOL.");
        }
    }
}
