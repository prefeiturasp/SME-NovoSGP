using MediatR;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    internal class ObterTodosAlunosNasTurmasQueryHandler : IRequestHandler<ObterTodosAlunosNasTurmasQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IRepositorioCache repositorioCache;

        public ObterTodosAlunosNasTurmasQueryHandler(IHttpClientFactory httpClientFactory, IRepositorioCache repositorioCache)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterTodosAlunosNasTurmasQuery request, CancellationToken cancellationToken)
        {
            var alunos = new List<AlunoPorTurmaResposta>();

            var chaveCache = $"todos-alunos-turma/ano-escolar:{request.AnoEscolar}";
            var cacheAlunos = request.AnoEscolar != "0" ? repositorioCache.Obter(chaveCache) : null;

            if (cacheAlunos != null)
                alunos = JsonConvert.DeserializeObject<IEnumerable<AlunoPorTurmaResposta>>(cacheAlunos).ToList();
            else
            {
                var httpClient = httpClientFactory.CreateClient("servicoEOL");
                var resposta = await httpClient.GetAsync($"turmas/todos-alunos/anoTurma/{request.AnoEscolar}/modalidade/{request.ModalidadeTurma}/anoLetivo/{request.AnoLetivo}");
                
                if (resposta.IsSuccessStatusCode)
                {
                    var json = await resposta.Content.ReadAsStringAsync();

                    if(json != "")
                    {
                        alunos = JsonConvert.DeserializeObject<IEnumerable<AlunoPorTurmaResposta>>(json).ToList();

                        // Salva em cache por 5 min
                        await repositorioCache.SalvarAsync(chaveCache, json, 5);
                    }
       
                }
            }

            return alunos;
        }
}
}
