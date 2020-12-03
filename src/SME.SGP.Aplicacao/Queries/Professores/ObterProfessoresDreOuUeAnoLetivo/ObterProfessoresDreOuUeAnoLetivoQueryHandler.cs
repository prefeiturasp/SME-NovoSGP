using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresDreOuUeAnoLetivoQueryHandler : IRequestHandler<ObterProfessoresDreOuUeAnoLetivoQuery, IEnumerable<ProfessorEolDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterProfessoresDreOuUeAnoLetivoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<ProfessorEolDto>> Handle(ObterProfessoresDreOuUeAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");

            var resposta = await httpClient.GetAsync($"/api/escolas/{request.CodigoDreUe}/professores/{request.AnoLetivo}");

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<ProfessorEolDto>>(json);
            }

            return Enumerable.Empty<ProfessorEolDto>();
        }
    }
}
