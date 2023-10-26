using System.Collections.Generic;
using System.Linq;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDresQueryHandler : IRequestHandler<ObterDresQuery, IEnumerable<DreRespostaEolDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterDresQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<DreRespostaEolDto>> Handle(ObterDresQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            
            var resposta = await httpClient.GetAsync(ServicosEolConstants.URL_DRES);
            
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<DreRespostaEolDto>>(json);
            }
            
            return Enumerable.Empty<DreRespostaEolDto>();
        }
    }
}