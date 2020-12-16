using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorUeQueryHandler : IRequestHandler<ObterComponentesCurricularesPorUeQuery, IEnumerable<ComponenteCurricularDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterComponentesCurricularesPorUeQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<ComponenteCurricularDto>> Handle(ObterComponentesCurricularesPorUeQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"v1/componentes-curriculares/ues/{request.UeCodigo}/turmas");

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<ComponenteCurricularDto>>(json);
            }
            else throw new NegocioException($"Não foi possível obter Componentes Curriculares para a ue [{request.UeCodigo}].");

        }
    }
}
