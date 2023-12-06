using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Aluno;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosResponsaveisAlunoEolQueryHandler : IRequestHandler<ObterDadosResponsaveisAlunoEolQuery, IEnumerable<DadosResponsavelAlunoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterDadosResponsaveisAlunoEolQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<DadosResponsavelAlunoDto>> Handle(ObterDadosResponsaveisAlunoEolQuery request, CancellationToken cancellationToken)
        {
            var alunos = Enumerable.Empty<DadosResponsavelAlunoDto>();

            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var url = string.Format(ServicosEolConstants.URL_DADOS_RESPONSAVEIS_FILIACAO_ALUNO, request.CodigoAluno);

            var resposta = await httpClient.GetAsync(url);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                alunos = JsonConvert.DeserializeObject<List<DadosResponsavelAlunoDto>>(json);
            }

            return alunos;
        }
    }
}
