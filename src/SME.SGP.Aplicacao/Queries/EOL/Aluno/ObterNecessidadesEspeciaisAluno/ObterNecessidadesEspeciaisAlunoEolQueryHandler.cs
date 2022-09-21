using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNecessidadesEspeciaisAlunoEolQueryHandler : IRequestHandler<ObterNecessidadesEspeciaisAlunoEolQuery, InformacoesEscolaresAlunoDto>
    {
        IHttpClientFactory httpClientFactory;

        public ObterNecessidadesEspeciaisAlunoEolQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<InformacoesEscolaresAlunoDto> Handle(ObterNecessidadesEspeciaisAlunoEolQuery request, CancellationToken cancellationToken)
        {
            var url = $@"alunos/{request.CodigoAluno}/necessidades-especiais";
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Não foram encontrados dados de necessidades especiais para o aluno no EOL");

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<InformacoesEscolaresAlunoDto>(json);
        }
    }
}
