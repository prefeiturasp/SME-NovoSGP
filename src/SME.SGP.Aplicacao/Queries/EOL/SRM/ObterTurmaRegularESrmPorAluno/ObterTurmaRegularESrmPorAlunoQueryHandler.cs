using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaRegularESrmPorAlunoQueryHandler : IRequestHandler<ObterTurmaRegularESrmPorAlunoQuery, IEnumerable<TurmasDoAlunoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterTurmaRegularESrmPorAlunoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<TurmasDoAlunoDto>> Handle(ObterTurmaRegularESrmPorAlunoQuery request, CancellationToken cancellationToken)
        {
            var alunos = Enumerable.Empty<TurmasDoAlunoDto>();

            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"alunos/paee/turma-srm-e-regular/aluno/{request.AlunoCodigo}", cancellationToken);
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
                alunos = JsonConvert.DeserializeObject<List<TurmasDoAlunoDto>>(json);
            }
            return alunos;
        }
    }
}
