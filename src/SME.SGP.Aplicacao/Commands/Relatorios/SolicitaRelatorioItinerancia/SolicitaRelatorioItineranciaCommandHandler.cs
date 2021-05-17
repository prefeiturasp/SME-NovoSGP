using MediatR;
using Newtonsoft.Json;
using Sentry;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SolicitaRelatorioItineranciaCommandHandler : IRequestHandler<SolicitaRelatorioItineranciaCommand, Guid>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public SolicitaRelatorioItineranciaCommandHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<Guid> Handle(SolicitaRelatorioItineranciaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient("servicoServidorRelatorios");

                var filtro = JsonConvert.SerializeObject(request.Filtro);
                var resposta = await httpClient.PostAsync($"api/v1/relatorios/sincronos/itinerancias", new StringContent(filtro, Encoding.UTF8, "application/json-patch+json"));

                if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Guid>(json);
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
            return Guid.Empty;
        }
    }
}
