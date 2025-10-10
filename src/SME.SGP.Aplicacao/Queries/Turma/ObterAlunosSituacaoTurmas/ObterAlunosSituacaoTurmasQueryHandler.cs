using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.SituacaoAluno;
using SME.SGP.Infra.Utilitarios;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosSituacaoTurmasQueryHandler : IRequestHandler<ObterAlunosSituacaoTurmasQuery, List<AlunosSituacaoTurmas>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterAlunosSituacaoTurmasQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }


        public async Task<List<AlunosSituacaoTurmas>> Handle(ObterAlunosSituacaoTurmasQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var url = ServicosEolConstants.URL_MATRICULA_TURMA_ESCOLA;

            var body = new
            {
                request.AnoLetivo,
                request.CodigoDre,
                request.SituacaoMatricula
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
                return JsonConvert.DeserializeObject<List<AlunosSituacaoTurmas>>(json);
            }

            return default;
        }
    }
}
