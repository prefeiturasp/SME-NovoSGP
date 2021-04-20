using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCiclosEolQueryHandler : IRequestHandler<ObterCiclosEolQuery, IEnumerable<CicloRetornoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterCiclosEolQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<IEnumerable<CicloRetornoDto>> Handle(ObterCiclosEolQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync("abrangencia/ciclo-ensino");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<CicloRetornoDto>>(json);
            }

            return default;
        }
    }
}
