using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
            var resposta = await httpClient.GetAsync($"alunos/ano-letivo/{request.AnoLetivo}/matriculados/quantidade?dreCodigo={request.DreCodigo}&ueCodigo={request.UeCodigo}&modalidade={request.Modalidade}&ano={request.AnoTurma}");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                quantidadeAlunosMatriculados = JsonConvert.DeserializeObject<List<QuantidadeAlunoMatriculadoDTO>>(json);
            }
            return quantidadeAlunosMatriculados;
        }
    }
}
