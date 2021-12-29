using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
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

            string baseAdd = string.Empty;
            try
            {
                var httpClient = httpClientFactory.CreateClient("servicoServidorRelatorios");
                baseAdd = httpClient.BaseAddress.ToString();
                var filtro = JsonConvert.SerializeObject(request.Filtro);
                var resposta = await httpClient.PostAsync($"api/v1/relatorios/sincronos/devolutivas", new StringContent(filtro, Encoding.UTF8, "application/json-patch+json"));

                if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Guid>(json);
                }
                else
                {
                    var guid = Guid.Empty;
                    throw new NegocioException($"Falha ao enviar para o servidor de relatorios {resposta.StatusCode}, [{resposta}], {baseAdd}");
                }
                
            }
            catch (Exception ex)
            {
                throw new NegocioException($"Falha ao enviar para o servidor de relatorios {ex.Message}, [{ex.StackTrace}], {baseAdd}");
            }

        }
    }
}
