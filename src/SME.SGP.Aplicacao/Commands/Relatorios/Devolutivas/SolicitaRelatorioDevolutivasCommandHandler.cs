using MediatR;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.Relatorios.Devolutivas
{
    public class SolicitaRelatorioDevolutivasCommandHandler : IRequestHandler<SolicitaRelatorioDevolutivasCommand, Guid>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public SolicitaRelatorioDevolutivasCommandHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<Guid> Handle(SolicitaRelatorioDevolutivasCommand request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoServidorRelatorios");
            var resposta = await httpClient.PostAsync($"api/v1/relatorios/sincronos/devolutivas/{request.DevolutivaId}", new StringContent(string.Empty,Encoding.UTF8, "application/json-patch+json"));

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Guid>(json);
            }

            return Guid.Empty;
        }
    }
}
