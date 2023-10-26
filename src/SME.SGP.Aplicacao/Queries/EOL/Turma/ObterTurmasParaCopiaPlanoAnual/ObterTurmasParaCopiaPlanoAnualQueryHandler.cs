using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasParaCopiaPlanoAnualQueryHandler : IRequestHandler<ObterTurmasParaCopiaPlanoAnualQuery, IEnumerable<TurmaParaCopiaPlanoAnualDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterTurmasParaCopiaPlanoAnualQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> Handle(ObterTurmasParaCopiaPlanoAnualQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var parametros = JsonConvert.SerializeObject(new
            {
                request.CodigoRf,
                request.ComponenteCurricular,
                CodigoTurma = request.TurmaId
            });

            var resposta = await httpClient.PostAsync(ServicosEolConstants.URL_FUNCIONARIOS_BURCAR_TURMAS_ELEGIVEIS, new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));
            var turmas = Enumerable.Empty<TurmaParaCopiaPlanoAnualDto>();
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                turmas = JsonConvert.DeserializeObject<List<TurmaParaCopiaPlanoAnualDto>>(json);
            }
            return turmas;
        }
    }
}
