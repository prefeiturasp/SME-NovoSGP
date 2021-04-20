using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTiposEscolaEolQueryHandler : IRequestHandler<ObterTiposEscolaEolQuery, IEnumerable<TipoEscolaRetornoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterTiposEscolaEolQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<IEnumerable<TipoEscolaRetornoDto>> Handle(ObterTiposEscolaEolQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync("escolas/tiposEscolas");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<TipoEscolaRetornoDto>>(json);
            }

            return default;
        }
    }
}
