using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunoPorTurmaAlunoCodigoQueryHandler : IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IRepositorioCache repositorioCache;

        public ObterAlunoPorTurmaAlunoCodigoQueryHandler(IHttpClientFactory httpClientFactory, IRepositorioCache repositorioCache)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.repositorioCache = repositorioCache;
        }

        public async Task<AlunoPorTurmaResposta> Handle(ObterAlunoPorTurmaAlunoCodigoQuery request, CancellationToken cancellationToken)
        {
            var alunos = new AlunoPorTurmaResposta();

            var chaveCache = string.Format(NomeChaveCache.ALUNO_TURMA, request.TurmaCodigo, request.AlunoCodigo, request.ConsideraInativos);
            var cacheAlunos = repositorioCache.Obter(chaveCache);
            if (cacheAlunos.NaoEhNulo())
                alunos = JsonConvert.DeserializeObject<AlunoPorTurmaResposta>(cacheAlunos);
            else
            {
                var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
                var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_TURMAS_ALUNO_CONSIDERA_INATIVOS, request.TurmaCodigo, request.AlunoCodigo, request.ConsideraInativos));
                if (resposta.IsSuccessStatusCode)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    alunos = JsonConvert.DeserializeObject<AlunoPorTurmaResposta>(json);

                    // Salva em cache por 5 min
                    await repositorioCache.SalvarAsync(chaveCache, json, 5);
                }
            }

            return alunos;
        }
    }
}
