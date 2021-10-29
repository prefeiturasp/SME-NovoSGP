using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Aluno.ObterAlunoPorTurmaDataPeriodoFim
{
    public class ObterAlunosPorTurmaDataPeriodoFimQueryHandler : IRequestHandler<ObterAlunosPorTurmaDataPeriodoFimQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IRepositorioCache repositorioCache;

        public ObterAlunosPorTurmaDataPeriodoFimQueryHandler(IHttpClientFactory httpClientFactory, IRepositorioCache repositorioCache)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.repositorioCache = repositorioCache;
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosPorTurmaDataPeriodoFimQuery request, CancellationToken cancellationToken)
        {
            var alunos = new List<AlunoPorTurmaResposta>();

            var chaveCache = $"alunos-turma-periodo-fim:{request.TurmaCodigo}";
            var cacheAlunos = repositorioCache.Obter(chaveCache);

            if (cacheAlunos != null)
            {
                alunos = JsonConvert.DeserializeObject<List<AlunoPorTurmaResposta>>(cacheAlunos);
            }
            else
            {
                var httpClient = httpClientFactory.CreateClient("servicoEOL");
                var resposta = await httpClient.GetAsync($"turmas/{request.TurmaCodigo}/data-periodo-fim-tiks/{request.DataPeriodoFim.Ticks}");
                if (resposta.IsSuccessStatusCode)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    alunos = JsonConvert.DeserializeObject<List<AlunoPorTurmaResposta>>(json);

                    // Salva em cache por 5 min
                    await repositorioCache.SalvarAsync(chaveCache, json, 5);
                }
            }

            return alunos;
        }
    }
}
