using MediatR;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresAutoCompleteQueryHandler : IRequestHandler<ObterProfessoresAutoCompleteQuery, IEnumerable<ProfessorResumoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterProfessoresAutoCompleteQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<IEnumerable<ProfessorResumoDto>> Handle(ObterProfessoresAutoCompleteQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var url = string.Format(ServicosEolConstants.URL_PROFESSORES_AUTOCOMPLETE, request.AnoLetivo, request.DreId);

            url += $"?nome={request.NomeProfessor}&ueId={request.UeId}";
            
            var resposta = await httpClient.GetAsync(url);
            
            if (!resposta.IsSuccessStatusCode)
                return default;

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                return default;

            var json = await resposta.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<IEnumerable<ProfessorResumoDto>>(json);
        }
    }
}
