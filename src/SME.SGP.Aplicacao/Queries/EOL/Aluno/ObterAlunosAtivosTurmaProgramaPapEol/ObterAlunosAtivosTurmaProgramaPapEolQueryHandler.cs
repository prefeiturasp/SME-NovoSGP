using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosAtivosTurmaProgramaPapEolQueryHandler: IRequestHandler<ObterAlunosAtivosTurmaProgramaPapEolQuery,IEnumerable<AlunosTurmaProgramaPapDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IRepositorioCache cache;

        public ObterAlunosAtivosTurmaProgramaPapEolQueryHandler(IHttpClientFactory httpClientFactory, IRepositorioCache cache)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<IEnumerable<AlunosTurmaProgramaPapDto>> Handle(ObterAlunosAtivosTurmaProgramaPapEolQuery request, CancellationToken cancellationToken)
        {
            if (request.AlunosCodigos.Length > 0)
            {
                var codigosOrdenados = string.Join("-", request.AlunosCodigos.OrderBy(c => c));
                var chaveCache = string.Format(NomeChaveCache.ALUNOS_PAP_ANO, request.AnoLetivo, codigosOrdenados);

                var cacheAlunos = await cache.ObterAsync(chaveCache);
                if (!string.IsNullOrEmpty(cacheAlunos))
                    return JsonConvert.DeserializeObject<IEnumerable<AlunosTurmaProgramaPapDto>>(cacheAlunos);

                var alunos = string.Join("&codigosAlunos=", request.AlunosCodigos);
                var url = string.Format(ServicosEolConstants.URL_ALUNOS_ALUNOS_PAP, request.AnoLetivo) + $"?codigosAlunos={alunos}";
                var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
                var resposta = await httpClient.GetAsync(url, cancellationToken);
                if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
                {
                    var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
                    await cache.SalvarAsync(chaveCache, json, 60);
                    return JsonConvert.DeserializeObject<IEnumerable<AlunosTurmaProgramaPapDto>>(json);
                }
            }
            return Enumerable.Empty<AlunosTurmaProgramaPapDto>();
        }
    }
}