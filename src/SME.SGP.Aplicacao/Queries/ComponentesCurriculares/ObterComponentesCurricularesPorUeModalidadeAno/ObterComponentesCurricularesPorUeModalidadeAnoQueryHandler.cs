using System;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorUeModalidadeAnoQueryHandler : IRequestHandler<ObterComponentesCurricularesPorUeModalidadeAnoQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;
        
        public ObterComponentesCurricularesPorUeModalidadeAnoQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesPorUeModalidadeAnoQuery request, CancellationToken cancellationToken)
        {
            var url = string.Empty;
            
            if (request.AnosEscolares.EhNulo() || !request.AnosEscolares.Any())
                url = string.Format(ServicosEolConstants.URL_COMPONENTES_CURRICULARES_UES_MODALIDADES_ANOS,request.CodigoUe,(int)request.Modalidade,request.AnoLetivo);
            else
            {
                url = string.Format(ServicosEolConstants.URL_COMPONENTES_CURRICULARES_UES_MODALIDADES_ANOS_ANOS_ESCOLARES, request.CodigoUe, (int)request.Modalidade, request.AnoLetivo);
                url += $"?anosEscolares={string.Join("&anosEscolares=", request.AnosEscolares)}";
            }

            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            
            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
                throw new NegocioException("Ocorreu um erro na tentativa de buscar as disciplinas no EOL.");

            var json = await resposta.Content.ReadAsStringAsync();
            var retorno = JsonConvert.DeserializeObject<List<ComponenteCurricularEol>>(json);
            if (!request.IgnorarInfoPedagogicasSgp)
            {
                var componentesCurricularesSgp = await mediator.Send(new ObterInfoPedagogicasComponentesCurricularesPorIdsQuery(retorno.ObterCodigos()));
                retorno.PreencherInformacoesPegagogicasSgp(componentesCurricularesSgp);
            }
            return retorno;
        }
    }
}
