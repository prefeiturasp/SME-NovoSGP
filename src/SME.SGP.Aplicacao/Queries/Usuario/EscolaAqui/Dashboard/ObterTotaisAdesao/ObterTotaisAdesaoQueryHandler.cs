using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra.Dtos.EscolaAqui.DashboardAdesao;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotaisAdesaoQueryHandler : IRequestHandler<ObterTotaisAdesaoQuery, IEnumerable<TotaisAdesaoResultado>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterTotaisAdesaoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<TotaisAdesaoResultado>> Handle(ObterTotaisAdesaoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoAcompanhamentoEscolar");
            var url = new StringBuilder(@"/api/v1/dashboard/adesao");

            if (!String.IsNullOrEmpty(request.CodigoDre))
                url.Append(@"?codigoDre=" + request.CodigoDre);

            if (!String.IsNullOrEmpty(request.CodigoDre) && !String.IsNullOrEmpty(request.CodigoUe))
                url.Append(@"&codigoUe=" + request.CodigoUe);

            var resposta = await httpClient.GetAsync($"{url}");

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<TotaisAdesaoResultado>>(json);
            }

            throw new Exception("Não foi possível obter dados de adesão do aplicativo.");
        }
    }
}