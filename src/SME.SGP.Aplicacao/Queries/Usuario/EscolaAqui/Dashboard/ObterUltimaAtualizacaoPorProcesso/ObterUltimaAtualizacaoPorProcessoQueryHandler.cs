using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimaAtualizacaoPorProcessoQueryHandler : IRequestHandler<ObterUltimaAtualizacaoPorProcessoQuery, UltimaAtualizaoWorkerPorProcessoResultado>
    {

        private readonly IHttpClientFactory httpClientFactory;

        public ObterUltimaAtualizacaoPorProcessoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<UltimaAtualizaoWorkerPorProcessoResultado> Handle(ObterUltimaAtualizacaoPorProcessoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoAcompanhamentoEscolar");
            var url = new StringBuilder(@"/api/v1/worker/ultimaAtualizacao");

            if (!String.IsNullOrEmpty(request.NomeProcesso))
                url.Append(@"?nomeProcesso=" + request.NomeProcesso);

            var resposta = await httpClient.GetAsync($"{url}");

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UltimaAtualizaoWorkerPorProcessoResultado>(json);
            }

            throw new Exception("Não foi possível obter dados de adesão do aplicativo.");
        }
    }
}
