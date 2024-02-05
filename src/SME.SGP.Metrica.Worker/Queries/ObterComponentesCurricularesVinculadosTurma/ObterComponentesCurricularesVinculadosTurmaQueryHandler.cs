using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.Queries
{
    public class ObterComponentesCurricularesVinculadosTurmaQueryHandler : IRequestHandler<ObterComponentesCurricularesVinculadosTurmaQuery, IEnumerable<string>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterComponentesCurricularesVinculadosTurmaQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<string>> Handle(ObterComponentesCurricularesVinculadosTurmaQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.GetAsync(ServicosEolConstants.URL_COMPONENTES_CURRICULARES_TURMAS_REGULARES + $"?codigoTurmas={request.CodigoTurma}", cancellationToken);
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
                var retorno = JsonConvert.DeserializeObject<List<ComponenteCurricularEol>>(json);
                return retorno.Select(cc => cc.Codigo.ToString());
            }

            return Enumerable.Empty<string>();
        }
    }
}
