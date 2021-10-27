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
    public class ObterMatriculasAlunoNaTurmaQueryHandler : IRequestHandler<ObterMatriculasAlunoNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterMatriculasAlunoNaTurmaQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterMatriculasAlunoNaTurmaQuery request, CancellationToken cancellationToken)
        {
            var alunos = new List<AlunoPorTurmaResposta>();
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"turmas/{request.CodigoTurma}/aluno/{request.CodigoAluno}/matriculas");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                alunos = JsonConvert.DeserializeObject<List<AlunoPorTurmaResposta>>(json);
            }
            return alunos;
        }
    }
}
