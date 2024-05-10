using MediatR;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
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
    internal class ObterTotalAlunosPorAnoModalidadeDreAnoLetivoInicioEFimQueryHandler : IRequestHandler<ObterTotalAlunosPorAnoModalidadeDreAnoLetivoInicioEFimQuery, long>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IRepositorioCache repositorioCache;

        public ObterTotalAlunosPorAnoModalidadeDreAnoLetivoInicioEFimQueryHandler(IHttpClientFactory httpClientFactory, IRepositorioCache repositorioCache)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }
        public async Task<long> Handle(ObterTotalAlunosPorAnoModalidadeDreAnoLetivoInicioEFimQuery request, CancellationToken cancellationToken)
        {
            long quantidadeAlunos = 0;

            var chaveCache = string.Format(NomeChaveCache.TOTAL_ALUNOS_DRE, request.AnoEscolar, request.ModalidadeTurma, request.CodigoDre, request.DataInicio.Ticks, request.DataFim.Ticks);
            var cacheAlunos = request.AnoEscolar != "0" && request.ModalidadeTurma > 0 ? repositorioCache.Obter(chaveCache) : null;

            if (cacheAlunos.NaoEhNulo())
                quantidadeAlunos = Convert.ToInt64(cacheAlunos);
            else
            {
                var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
                var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_TURMAS_TODOS_ALUNOS_ANO_MODALIDADE_ANO_LETIVO_DRE_INICIO_FIM, 
                                                                       request.AnoEscolar, 
                                                                       request.ModalidadeTurma,
                                                                       request.AnoLetivo,
                                                                       request.CodigoDre,
                                                                       request.DataInicio.Ticks,
                                                                       request.DataFim.Ticks));
                
                if (resposta.IsSuccessStatusCode)
                {
                    var resultado = await resposta.Content.ReadAsStringAsync();

                    if(resultado != "")
                    {
                        quantidadeAlunos = Convert.ToInt64(resultado);

                        // Salva em cache por 5 min
                        await repositorioCache.SalvarAsync(chaveCache, resultado, 5);
                    }
       
                }
            }

            return quantidadeAlunos;
        }
}
}
