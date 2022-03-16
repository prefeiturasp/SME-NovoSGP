using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorUeAnosModalidadeQueryHandler : IRequestHandler<ObterComponentesCurricularesPorUeAnosModalidadeQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IHttpClientFactory httpClientFactory;        

        public ObterComponentesCurricularesPorUeAnosModalidadeQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));            
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesPorUeAnosModalidadeQuery request, CancellationToken cancellationToken)
        {

            var turmas = String.Join("&codigoTurmas=", request.TurmaCodigos);

            using (var httpClient = httpClientFactory.CreateClient("servicoEOL"))
            {
                var resposta = await httpClient.GetAsync($"/api/v1/componentes-curriculares/turmas?codigoTurmas={turmas}");

                if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    var componentes = JsonConvert.DeserializeObject<IEnumerable<ComponenteCurricularEol>>(json);
                    return componentes.OrderBy(c => c.Descricao);
                }
                else throw new NegocioException("Não foi possível obter Componentes Curriculares.");
            }
        }

    }
}
