using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNumeroEstudantesPap
{
    public class PainelEducacionalIndicadoresPapEolQueryHandler : IRequestHandler<PainelEducacionalIndicadoresPapEolQuery, IEnumerable<ContagemNumeroAlunosPapDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public PainelEducacionalIndicadoresPapEolQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<ContagemNumeroAlunosPapDto>> Handle(PainelEducacionalIndicadoresPapEolQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var url = string.Format(ServicosEolConstants.URL_PAINEL_EDUCACIONAL_INDICADORES_PAP, 
                request.CodigoDre ?? "", request.CodigoUe ?? "");

            var resposta = await httpClient.GetAsync(url);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<ContagemNumeroAlunosPapDto>>(json);
            }

            if (resposta.StatusCode == HttpStatusCode.NoContent)
            {
                return new List<ContagemNumeroAlunosPapDto>();
            }

            throw new NegocioException($"Erro ao obter informações PAP do painel educacional no EOL. Status: {resposta.StatusCode}");
        }
    }
}