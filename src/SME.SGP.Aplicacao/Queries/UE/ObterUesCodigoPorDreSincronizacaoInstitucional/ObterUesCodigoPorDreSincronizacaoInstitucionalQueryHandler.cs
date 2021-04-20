using MediatR;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUesCodigoPorDreSincronizacaoInstitucionalQueryHandler : IRequestHandler<ObterUesCodigoPorDreSincronizacaoInstitucionalQuery, IEnumerable<string>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterUesCodigoPorDreSincronizacaoInstitucionalQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
        }
        async public Task<IEnumerable<string>> Handle(ObterUesCodigoPorDreSincronizacaoInstitucionalQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"dres/{request.DreCodigo}/ues");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<string>>(json);
            }

            return default;
        }
    }
}
