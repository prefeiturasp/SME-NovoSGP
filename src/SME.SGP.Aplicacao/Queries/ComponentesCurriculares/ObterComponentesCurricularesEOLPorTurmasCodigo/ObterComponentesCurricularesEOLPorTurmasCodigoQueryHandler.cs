using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandler : IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<ComponenteCurricularDto>> Handle(ObterComponentesCurricularesEOLPorTurmasCodigoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");

            var turmas = String.Join("&codigoTurmas=", request.CodigosDeTurmas);

            var resposta = await httpClient.GetAsync($"/api/v1/componentes-curriculares/turmas?codigoTurmas={turmas}{(request.AdicionarComponentesPlanejamento.HasValue ? $"&adicionarComponentesPlanejamento={request.AdicionarComponentesPlanejamento.Value}" : string.Empty)}");

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<ComponenteCurricularDto>>(json);
            }
            else throw new NegocioException("Não foi possível obter Componentes Curriculares.");

        }
    }
}
