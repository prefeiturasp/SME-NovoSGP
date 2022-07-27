using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesRegenciaPorAnoQueryHandler : IRequestHandler<ObterComponentesRegenciaPorAnoQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly HttpClient httpClient;
        public ObterComponentesRegenciaPorAnoQueryHandler(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesRegenciaPorAnoQuery request, CancellationToken cancellationToken)
        {
            var url = $"v1/componentes-curriculares/anos/{request.AnoTurma}/regencia";

            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                throw new NegocioException("Ocorreu um erro na tentativa de buscar as disciplinas no EOL.");
            }

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ComponenteCurricularEol>>(json);
        }
    }
}
