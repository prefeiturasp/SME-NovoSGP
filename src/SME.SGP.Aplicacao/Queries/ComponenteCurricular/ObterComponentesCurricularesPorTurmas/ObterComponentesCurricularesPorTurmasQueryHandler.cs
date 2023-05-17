using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorTurmasQueryHandler : IRequestHandler<ObterComponentesCurricularesPorTurmasQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterComponentesCurricularesPorTurmasQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesPorTurmasQuery request, CancellationToken cancellationToken)
        {
            var codigosTurmas = string.Join("&codigoTurmas=", request.CodigosTurmas);

            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"v1/componentes-curriculares/turmas/regulares?codigoTurmas={codigosTurmas}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ComponenteCurricularEol>>(json);
            }

            return Enumerable.Empty<ComponenteCurricularEol>();
        }
    }
}
