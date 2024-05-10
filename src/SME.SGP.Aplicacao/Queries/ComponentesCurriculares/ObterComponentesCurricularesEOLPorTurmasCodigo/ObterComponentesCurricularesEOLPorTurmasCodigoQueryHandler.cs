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
    public class ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandler : IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesEOLPorTurmasCodigoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var turmas = String.Join("&codigoTurmas=", request.CodigosDeTurmas);

            var resposta = await httpClient.GetAsync(ServicosEolConstants.URL_COMPONENTES_CURRICULARES_TURMAS + $"?codigoTurmas={turmas}{(request.AdicionarComponentesPlanejamento.HasValue ? $"&adicionarComponentesPlanejamento={request.AdicionarComponentesPlanejamento.Value}" : string.Empty)}");

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                var retorno = JsonConvert.DeserializeObject<List<ComponenteCurricularEol>>(json);

                var componentesCurricularesSgp = await mediator.Send(new ObterInfoPedagogicasComponentesCurricularesPorIdsQuery(retorno.ObterCodigos()));
                retorno.PreencherInformacoesPegagogicasSgp(componentesCurricularesSgp);
                return retorno;

            }
            else throw new NegocioException("Não foi possível obter Componentes Curriculares.");

        }
    }
}
