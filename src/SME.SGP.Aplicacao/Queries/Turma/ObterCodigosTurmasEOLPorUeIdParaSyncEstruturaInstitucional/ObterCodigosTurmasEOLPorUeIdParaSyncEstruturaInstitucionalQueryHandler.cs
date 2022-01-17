using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            var turmasCodigo = new List<long>();
            var anosLetivosVigentes = String.Join("&anosLetivosVigente=", request.AnosLetivos);

            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            httpClient.Timeout = TimeSpan.FromMinutes(4);

            var resposta = await httpClient.GetAsync($"turmas/ue/{request.UeId}/sincronizacoes-institucionais/anosLetivos?anosLetivosVigente={anosLetivosVigentes}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                turmasCodigo = JsonConvert.DeserializeObject<List<long>>(json);
            }
            return turmasCodigo;
        }
    }
}
