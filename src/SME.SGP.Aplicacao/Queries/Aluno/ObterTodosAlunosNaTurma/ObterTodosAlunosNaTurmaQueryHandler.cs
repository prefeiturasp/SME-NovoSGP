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
using SME.SGP.Dominio.Constantes;

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

            var chaveCache = string.Format(NomeChaveCache.CHAVE_ALUNOS_TURMA, request.CodigoTurma, request.CodigoAluno?.ToString());
            return await repositorioCache.ObterAsync(chaveCache,
                async () => await BuscarAlunosTurma(request.CodigoTurma, request.CodigoAluno),
                request.TempoArmazenamentoCache);
        }

        private async Task<IEnumerable<AlunoPorTurmaResposta>> BuscarAlunosTurma(int codigoTurma, int? codigoAluno)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"turmas/{codigoTurma}/todos-alunos{(codigoAluno.HasValue ? $"?codigoAluno={codigoAluno.Value}" : string.Empty)}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<AlunoPorTurmaResposta>>(json).ToList();
            }

            return Enumerable.Empty<AlunoPorTurmaResposta>();
        }
    }
}
