using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAdministradoresPorUEQueryHandler : IRequestHandler<ObterAdministradoresPorUEQuery, string[]>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterAdministradoresPorUEQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<string[]> Handle(ObterAdministradoresPorUEQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            
            var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_ESCOLAS_ADMINISTRADOR_SGP,request.CodigoUe));

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<string[]>(json);
            }

            return default;
        }
    }
}
