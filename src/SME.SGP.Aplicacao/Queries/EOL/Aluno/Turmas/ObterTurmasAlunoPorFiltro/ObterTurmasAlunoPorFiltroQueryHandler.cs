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
    public class ObterTurmasAlunoPorFiltroQueryHandler : IRequestHandler<ObterTurmasAlunoPorFiltroQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        public ObterTurmasAlunoPorFiltroQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterTurmasAlunoPorFiltroQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var url = $"alunos/{request.CodidoAluno}/turmas/anosLetivos/{request.AnoLetivo}/matriculaTurma/{request.FiltrarSituacaoMatricula}";
            try
            {
                var resposta = await httpClient.GetAsync(url);
                if (resposta.IsSuccessStatusCode)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<AlunoPorTurmaResposta>>(json);
                }
                else
                {
                    string erro = $"Não foi possível obter os dados do aluno no EOL - HttpCode {(int)resposta.StatusCode}";
                    SentrySdk.AddBreadcrumb(erro);
                    throw new NegocioException(erro);
                }
            }
            catch (Exception e)
            {
                SentrySdk.CaptureMessage($"Erro ao obter os dados do aluno no EOL - Código:{request.CodidoAluno}, Ano:{request.AnoLetivo}, FiltrarSituacaoMatricula:{request.FiltrarSituacaoMatricula} - Erro:{e.Message}");
                SentrySdk.CaptureException(e);
                throw e;
            }
        }
    }
}
