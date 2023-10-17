using System;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesEolQueryHandler : IRequestHandler<ObterComponentesCurricularesEolQuery, IEnumerable<ComponenteCurricularDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterComponentesCurricularesEolQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<ComponenteCurricularDto>> Handle(ObterComponentesCurricularesEolQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            
            var resposta = await httpClient.GetAsync(ServicosEolConstants.URL_COMPONENTES_CURRICULARES);

            if (!resposta.IsSuccessStatusCode)
                return null;

            var retorno = await resposta.Content.ReadAsStringAsync();

            var componentes = JsonConvert.DeserializeObject<IEnumerable<ComponenteCurricularDto>>(retorno);
            
            if (componentes.EhNulo() || !componentes.Any())
                throw new NegocioException(MensagemNegocioEOL.COMPONENTE_CURRICULAR_NAO_LOCALIZADO);
            
            return componentes;
        }
    }
}
