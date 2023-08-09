using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesRegenciaPorAnoEolQueryHandler : IRequestHandler<ObterComponentesRegenciaPorAnoEolQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;
        public ObterComponentesRegenciaPorAnoEolQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesRegenciaPorAnoEolQuery request, CancellationToken cancellationToken)
        {
            var url = $"v1/componentes-curriculares/anos/{request.AnoTurma}/regencia";
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                throw new NegocioException("Ocorreu um erro na tentativa de buscar as disciplinas no EOL.");
            }

            var json = await resposta.Content.ReadAsStringAsync();
            var retorno = JsonConvert.DeserializeObject<List<ComponenteCurricularEol>>(json);
            var componentesCurricularesSgp = await mediator.Send(new ObterInfoPedagogicasComponentesCurricularesPorIdsQuery(retorno.ObterCodigos()));
            retorno.PreencherInformacoesPegagogicasSgp(componentesCurricularesSgp);
            return retorno;
        }
    }
}
