using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAlunosEolMatriculadosQueryHandler : IRequestHandler<ObterQuantidadeAlunosEolMatriculadosQuery, IEnumerable<QuantidadeAlunoMatriculadoDTO>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterQuantidadeAlunosEolMatriculadosQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<QuantidadeAlunoMatriculadoDTO>> Handle(ObterQuantidadeAlunosEolMatriculadosQuery request, CancellationToken cancellationToken)
        {
            var quantidadeAlunosMatriculados = new List<QuantidadeAlunoMatriculadoDTO>();

            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            
            var parametros = "";
            
            if (!string.IsNullOrEmpty(request.DreCodigo) && !request.DreCodigo.Contains("-99"))
                parametros += $"dreCodigo={request.DreCodigo}";

            if (!string.IsNullOrEmpty(request.UeCodigo) && !request.UeCodigo.Contains("-99"))
                parametros += $"&ueCodigo={request.UeCodigo}";

            if (request.Modalidade > 0)
                parametros += $"&modalidade={request.Modalidade}";

            if (!string.IsNullOrEmpty(request.AnoTurma) && !request.AnoTurma.Contains("-99"))
                parametros += $"&ano={request.AnoTurma}";

            if (parametros.StartsWith("&"))
                parametros = parametros.Substring(1);
            var resposta = await httpClient.GetAsync($"alunos/ano-letivo/{request.AnoLetivo}/matriculados/quantidade" + (parametros.Length > 0 ? $"?{parametros}" : ""));

           
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                quantidadeAlunosMatriculados = JsonConvert.DeserializeObject<List<QuantidadeAlunoMatriculadoDTO>>(json);
            }
            return quantidadeAlunosMatriculados;
        }
    }
}
