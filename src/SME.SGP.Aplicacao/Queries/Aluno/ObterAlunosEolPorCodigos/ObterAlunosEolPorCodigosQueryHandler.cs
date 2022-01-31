using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Aluno.ObterAlunosEolPorCodigos
{
    public class ObterAlunosEolPorCodigosQueryHandler : IRequestHandler<ObterAlunosEolPorCodigosQuery, IEnumerable<TurmasDoAlunoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterAlunosEolPorCodigosQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<TurmasDoAlunoDto>> Handle(ObterAlunosEolPorCodigosQuery request, CancellationToken cancellationToken)
        {
            var alunos = new List<TurmasDoAlunoDto>();

            var codigosAlunos = String.Join("&codigosAluno=", request.CodigosAluno);

            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"alunos/alunos?codigosAluno={codigosAlunos}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                alunos = JsonConvert.DeserializeObject<List<TurmasDoAlunoDto>>(json);
            }
            return alunos;
        }
    }
}
