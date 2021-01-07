using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNecessidadesEspeciaisAlunoQueryHandler : IRequestHandler<ObterNecessidadesEspeciaisAlunoQuery, InformacoesEscolaresAlunoDto>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterNecessidadesEspeciaisAlunoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<InformacoesEscolaresAlunoDto> Handle(ObterNecessidadesEspeciaisAlunoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");

            var url = new StringBuilder("alunos/" + request.CodigoAluno + "/necessidades-especiais");

            var resposta = await httpClient.GetAsync($"{url}", cancellationToken);

            if (!resposta.IsSuccessStatusCode || resposta.StatusCode == HttpStatusCode.NoContent)
                throw new NegocioException("Não foram encontrados dados de necessidades especiais para o aluno.", HttpStatusCode.InternalServerError);

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<InformacoesEscolaresAlunoDto>(json);
        }
    }
}