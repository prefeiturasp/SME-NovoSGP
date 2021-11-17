using MediatR;
using Newtonsoft.Json;
using System;
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
            Console.WriteLine(" ********************************  Inicio Solicitação Relatório Itinerância ****************************************** ");
            var httpClient = httpClientFactory.CreateClient("servicoServidorRelatorios");

            Console.WriteLine($"Path - {httpClient.BaseAddress.OriginalString} ");
            Console.WriteLine($"DefaultRequestHeaders - {httpClient.DefaultRequestHeaders.ToString()} ");
            var filtro = JsonConvert.SerializeObject(request.Filtro);
            var resposta = await httpClient.PostAsync($"api/v1/relatorios/sincronos/itinerancias", new StringContent(filtro, Encoding.UTF8, "application/json-patch+json"));
            Console.WriteLine($"Resposta - {resposta.StatusCode} ");
            Console.WriteLine(" ********************************  Fim Solicitação Relatório Itinerância ****************************************** ");
            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Guid>(json);
            }
            return Guid.Empty;
        }
    }
}
