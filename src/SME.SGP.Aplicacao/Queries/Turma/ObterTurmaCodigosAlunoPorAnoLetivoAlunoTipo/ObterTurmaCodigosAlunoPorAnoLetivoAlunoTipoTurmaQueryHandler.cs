using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQueryHandler : IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<string[]> Handle(ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery request, CancellationToken cancellationToken)
        {
            var tiposTurma = String.Join("&tiposTurma=", request.TiposTurmas);
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var data = request.DataReferencia ?? DateTime.Today;
            var url = string.Format(ServicosEolConstants.URL_TURMAS_ANOS_LETIVOS_ALUNOS_REGULARES, request.AnoLetivo, request.CodigoAluno) + $"?tiposTurma={tiposTurma}&dataReferencia={data:yyyy-MM-dd}{(!string.IsNullOrWhiteSpace(request.UeCodigo) ? $"&ueCodigo={request.UeCodigo}" : string.Empty)}{(request.Semestre.HasValue ? $"&semestre={request.Semestre}" : string.Empty)}";
            var resposta = await httpClient.GetAsync(url);
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<string[]>(json);
            }
            return new string[] { };
        }
    }
}
