using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosEolPorTurmaQueryHandler :IRequestHandler<ObterAlunosEolPorTurmaQuery,IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IRepositorioCache cache;
        

        public ObterAlunosEolPorTurmaQueryHandler(IHttpClientFactory httpClientFactory,IRepositorioCache cache)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        private string ObterChaveCacheAlunosTurma(string turmaId) => $"alunos-turma:{turmaId}";
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosEolPorTurmaQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var alunos = new List<AlunoPorTurmaResposta>();

            var chaveCache = ObterChaveCacheAlunosTurma(request.TurmaId);
            var cacheAlunos = cache.Obter(chaveCache);

            if (cacheAlunos != null)
            {
                alunos = JsonConvert.DeserializeObject<List<AlunoPorTurmaResposta>>(cacheAlunos);
            }
            else
            {
                var resposta = await httpClient.GetAsync($"turmas/{request.TurmaId}/considera-inativos/{request.ConsideraInativos}");
                if (resposta.IsSuccessStatusCode)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    alunos = JsonConvert.DeserializeObject<List<AlunoPorTurmaResposta>>(json);

                    // Salva em cache por 5 min
                    await cache.SalvarAsync(chaveCache, json, 5);
                }
            }

            return alunos;
        }
    }
}