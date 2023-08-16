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
    public class ObterCodigosTurmasEOLPorUeIdParaSyncEstruturaInstitucionalQueryHandler : IRequestHandler<ObterCodigosTurmasEOLPorUeIdParaSyncEstruturaInstitucionalQuery, IEnumerable<long>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterCodigosTurmasEOLPorUeIdParaSyncEstruturaInstitucionalQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<long>> Handle(ObterCodigosTurmasEOLPorUeIdParaSyncEstruturaInstitucionalQuery request, CancellationToken cancellationToken)
        {
            var turmasCodigo = Enumerable.Empty<long>();
            var anosLetivosVigentes = String.Join("&anosLetivosVigente=", request.AnosLetivos);

            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            httpClient.Timeout = TimeSpan.FromMinutes(4);

            var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_TURMAS_UE_SINCRONIZACOES_ANO_LETIVO, request.UeId) + $"?anosLetivosVigente={anosLetivosVigentes}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                turmasCodigo = JsonConvert.DeserializeObject<List<long>>(json);
            }
            return turmasCodigo;
        }
    }
}
