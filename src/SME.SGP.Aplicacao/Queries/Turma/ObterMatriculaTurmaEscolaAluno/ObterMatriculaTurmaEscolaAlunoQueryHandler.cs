using MediatR;
using Newtonsoft.Json;
using SME.Pedagogico.Interface.DTO.Turma;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterMatriculaTurmaEscolaAlunoQueryHandler : IRequestHandler<ObterMatriculaTurmaEscolaAlunoQuery, List<AlunoMatriculaTurmaEscolaDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterMatriculaTurmaEscolaAlunoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }


        public async Task<List<AlunoMatriculaTurmaEscolaDto>> Handle(ObterMatriculaTurmaEscolaAlunoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var url = ServicosEolConstants.URL_MATRICULA_TURMA_ESCOLA_ALUNO;

            var body = new
            {
                request.AnoLetivo,
                request.CodigoDre,
                request.SituacaoMatricula,
                request.Turmas
            };

            var jsonBody = JsonConvert.SerializeObject(body, new JsonSerializerSettings
            {
                ContractResolver = new NullToEmptyStringResolver()
            });

            var resposta = await httpClient.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, "application/json"));

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException($"Não foi possível consultar as matriculas dos alunos. {resposta.StatusCode}");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<AlunoMatriculaTurmaEscolaDto>>(json);
            }

            return default;
        }
    }
}
