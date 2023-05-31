using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaCodigosAlunoPorAnoLetivoUeTipoTurmaQueryHandler  : IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoUeTipoTurmaQuery, string[]>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterTurmaCodigosAlunoPorAnoLetivoUeTipoTurmaQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<string[]> Handle(ObterTurmaCodigosAlunoPorAnoLetivoUeTipoTurmaQuery request, CancellationToken cancellationToken)
        {
            var tiposTurma = string.Join("&tiposTurma=", request.TiposTurmas);
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var data = request.DataReferencia ?? DateTime.Today;
            var resposta = await httpClient.GetAsync($"turmas/anos-letivos/{request.AnoLetivo}/ue/{request.UeCodigo}/regulares?tiposTurma={tiposTurma}&consideraHistorico={request.ConsideraHistorico}&dataReferencia={data:yyyy-MM-dd}&semestre={request.Semestre}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<string[]>(json);
            }
            return new string[] { };
        }
    }
}