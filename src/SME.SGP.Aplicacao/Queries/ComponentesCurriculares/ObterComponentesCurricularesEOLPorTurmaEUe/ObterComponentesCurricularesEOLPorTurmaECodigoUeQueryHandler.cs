using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesEOLPorTurmaECodigoUeQueryHandler : IRequestHandler<ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery, IEnumerable<ComponenteCurricularDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterComponentesCurricularesEOLPorTurmaECodigoUeQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<ComponenteCurricularDto>> Handle(ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var queryParamTurmas = string.Empty;

            if (request.CodigosDeTurmas != null && request.CodigosDeTurmas.Any())
            {
                var codigosTurmas = String.Join("&turmas=", request.CodigosDeTurmas);
                queryParamTurmas = $"?turmas={codigosTurmas}";
            }

            var resposta = await httpClient.GetAsync($"/api/v1/componentes-curriculares/ues/{request.CodigoUe}/turmas{queryParamTurmas}");

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<ComponenteCurricularDto>>(json);
            }
            else throw new NegocioException("Não foi possível obter Componentes Curriculares.");

        }
    }
}
