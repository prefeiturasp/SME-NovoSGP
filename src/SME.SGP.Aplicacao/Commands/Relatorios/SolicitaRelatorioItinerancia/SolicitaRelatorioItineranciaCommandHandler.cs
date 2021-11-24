using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio.Interfaces.Repositorios;
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
        private readonly IRepositorioTesteLog repositorioTesteLog;

        public SolicitaRelatorioItineranciaCommandHandler(IHttpClientFactory httpClientFactory, IRepositorioTesteLog repositorioTesteLog)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.repositorioTesteLog = repositorioTesteLog ?? throw new ArgumentNullException(nameof(repositorioTesteLog));
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

                    await repositorioTesteLog.Gravar($"DefaultRequestHeaders - {httpClient.DefaultRequestHeaders.ToString()} - StatusCode de Resposta - {resposta.StatusCode.ToString()} - Código correlação : {json}");
                    return JsonConvert.DeserializeObject<Guid>(json);
                }
                var mensagem = $"DefaultRequestHeaders - {httpClient.DefaultRequestHeaders.ToString()} - StatusCode de Resposta - {resposta.StatusCode.ToString()}";
                await repositorioTesteLog.Gravar(mensagem);
                return Guid.Empty;
            }
            catch (Exception ex)
            {
                await repositorioTesteLog.Gravar($"SolicitaRelatorioItineranciaCommand Error - {ex.ToString()}");
                throw;
            }
        }
    }
}
