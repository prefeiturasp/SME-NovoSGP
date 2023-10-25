using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresPorRfQueryHandler : IRequestHandler<ObterProfessoresPorRfQuery,IEnumerable<ProfessorResumoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterProfessoresPorRfQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<ProfessorResumoDto>> Handle(ObterProfessoresPorRfQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var url = string.Format(ServicosEolConstants.URL_PROFESSORES_BUSCAR_POR_LISTA_RF, request.AnoLetivo);

            var resposta = await httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(request.CodigosRF), Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode)
                return default;

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                return default;

            var json = await resposta.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<ProfessorResumoDto>>(json);
        }
        
    }
}