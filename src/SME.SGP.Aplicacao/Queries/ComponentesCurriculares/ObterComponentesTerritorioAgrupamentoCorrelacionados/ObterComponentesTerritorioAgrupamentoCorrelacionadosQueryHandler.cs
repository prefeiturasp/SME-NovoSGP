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
    public class ObterComponentesTerritorioAgrupamentoCorrelacionadosQueryHandler : IRequestHandler<ObterComponentesTerritorioAgrupamentoCorrelacionadosQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterComponentesTerritorioAgrupamentoCorrelacionadosQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator)); ;
        }
                                                                       
        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesTerritorioAgrupamentoCorrelacionadosQuery request, CancellationToken cancellationToken)
        {
            var componenteCurricularEol = new List<ComponenteCurricularEol>();
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var parametros = JsonConvert.SerializeObject(request.CodigosComponentesCurricularesAgrupamentoTerritorioSaber);
            var url = ServicosEolConstants.URL_COMPONENTES_CURRICULARES_AGRUPAMENTOS_TERRITORIO_SABER_CORRELACIONADOS;
            if (request.DataReferencia.HasValue)
                url += $"?dataBaseTick={request.DataReferencia.Value.Ticks}";

            var resposta = await httpClient.PostAsync(url,
                                                    new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                componenteCurricularEol = JsonConvert.DeserializeObject<List<ComponenteCurricularEol>>(json);
                var componentesCurricularesSgp = await mediator.Send(new ObterInfoPedagogicasComponentesCurricularesPorIdsQuery(componenteCurricularEol.ObterCodigos()));
                componenteCurricularEol.PreencherInformacoesPegagogicasSgp(componentesCurricularesSgp);
                return componenteCurricularEol;
            }
            return componenteCurricularEol;
        }
    }
}
