using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAjudasDoSistemaQueryHandler : IRequestHandler<ObterAjudasDoSistemaQuery, IEnumerable<AjudaDoSistemaDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterAjudasDoSistemaQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<AjudaDoSistemaDto>> Handle(ObterAjudasDoSistemaQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync("ajudadosistema/sgp", cancellationToken);

            var ajudas = Enumerable.Empty<AjudaDoSistemaDto>();

            if (!resposta.IsSuccessStatusCode) 
                return ajudas;
            
            var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
            ajudas = JsonConvert.DeserializeObject<IEnumerable<AjudaDoSistemaDto>>(json);

            return ajudas;
        }
    }
}