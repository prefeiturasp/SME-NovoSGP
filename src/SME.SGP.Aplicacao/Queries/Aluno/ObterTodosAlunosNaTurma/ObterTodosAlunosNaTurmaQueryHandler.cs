using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SME.SGP.Aplicacao
{
    public class ObterTodosAlunosNaTurmaQueryHandler : IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IRepositorioCache repositorioCache;

        public ObterTodosAlunosNaTurmaQueryHandler(IHttpClientFactory httpClientFactory, IRepositorioCache repositorioCache)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterTodosAlunosNaTurmaQuery request, CancellationToken cancellationToken)
        {
            var alunos = new List<AlunoPorTurmaResposta>();

            var chaveCache = $"todos-alunos-turma:{request.CodigoTurma}/codigo-aluno:{request.CodigoAluno}";
            var cacheAlunos = repositorioCache.Obter(chaveCache);
            if (cacheAlunos != null)
                alunos = JsonConvert.DeserializeObject<IEnumerable<AlunoPorTurmaResposta>>(cacheAlunos).ToList();
            else
            {
                var httpClient = httpClientFactory.CreateClient("servicoEOL");
                var resposta = await httpClient.GetAsync($"turmas/{request.CodigoTurma}/todos-alunos{(request.CodigoAluno.HasValue ? $"?codigoAluno={request.CodigoAluno.Value}" : string.Empty)}");
                if (resposta.IsSuccessStatusCode)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    alunos = JsonConvert.DeserializeObject<IEnumerable<AlunoPorTurmaResposta>>(json).ToList();

                    // Salva em cache por 5 min
                    await repositorioCache.SalvarAsync(chaveCache, json, 5);
                }
            }

            return alunos;
        }
    }
}
