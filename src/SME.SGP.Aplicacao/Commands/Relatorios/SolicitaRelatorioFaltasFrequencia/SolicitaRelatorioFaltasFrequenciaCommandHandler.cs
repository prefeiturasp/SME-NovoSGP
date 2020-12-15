using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.Relatorios.SolicitaRelatorioFaltasFrequencia
{
    public class SolicitaRelatorioFaltasFrequenciaCommandHandler : IRequestHandler<SolicitaRelatorioFaltasFrequenciaCommand, Guid>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public SolicitaRelatorioFaltasFrequenciaCommandHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<Guid> Handle(SolicitaRelatorioFaltasFrequenciaCommand request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoServidorRelatorios");

            var filtro = JsonConvert.SerializeObject(request.Filtro);
            var resposta = await httpClient.PostAsync($"api/v1/relatorios/sincronos/faltas-frequencia", new StringContent(filtro, Encoding.UTF8, "application/json-patch+json"));

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Guid>(json);
            }

            return Guid.Empty;
        }
    }
}
