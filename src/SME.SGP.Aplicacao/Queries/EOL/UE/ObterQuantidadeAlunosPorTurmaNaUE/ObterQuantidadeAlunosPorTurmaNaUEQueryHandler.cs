using MediatR;
using Newtonsoft.Json;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAlunosPorTurmaNaUEQueryHandler : IRequestHandler<ObterQuantidadeAlunosPorTurmaNaUEQuery, IEnumerable<AlunosPorTurmaDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        public ObterQuantidadeAlunosPorTurmaNaUEQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<AlunosPorTurmaDto>> Handle(ObterQuantidadeAlunosPorTurmaNaUEQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var url = $"escolas/{request.UeCodigo}/alunos/quantidade";

            var resposta = await httpClient.GetAsync(url);
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<AlunosPorTurmaDto>>(json);
            }
            else
            {
                string erro = $"Não foi possível obter quantidade de alunos da turma no EOL - HttpCode {(int)resposta.StatusCode}";
                SentrySdk.AddBreadcrumb(erro);
                throw new NegocioException(erro);
            }
        }
    }
}
