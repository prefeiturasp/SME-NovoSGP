using MediatR;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPerfisPorLoginQueryHandler : IRequestHandler<ObterPerfisPorLoginQuery, PerfisApiEolDto>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterPerfisPorLoginQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<PerfisApiEolDto> Handle(ObterPerfisPorLoginQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            
            var url = string.Format(ServicosEolConstants.URL_AUTENTICACAO_SGP_CARREGAR_PERFIS_POR_LOGIN,request.Login);

            var resposta = await httpClient.GetAsync(url);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PerfisApiEolDto>(json);
            }
            return default;
        }
    }
}
