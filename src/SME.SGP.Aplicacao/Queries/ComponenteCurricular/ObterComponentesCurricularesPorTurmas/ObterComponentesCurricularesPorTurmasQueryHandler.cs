using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorTurmasQueryHandler : IRequestHandler<ObterComponentesCurricularesPorTurmasQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterComponentesCurricularesPorTurmasQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator)); ;
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesPorTurmasQuery request, CancellationToken cancellationToken)
        {
            var codigosTurmas = string.Join("&codigoTurmas=", request.CodigosTurmas);

            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"v1/componentes-curriculares/turmas/regulares?codigoTurmas={codigosTurmas}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                var retorno = JsonConvert.DeserializeObject<List<ComponenteCurricularEol>>(json);

                var componentesCurricularesSgp = await mediator.Send(new ObterInfoPedagogicasComponentesCurricularesPorIdsQuery(retorno.ObterCodigos()));
                retorno.PreencherInformacoesPegagogicasSgp(componentesCurricularesSgp);
                return retorno;
            }

            return Enumerable.Empty<ComponenteCurricularEol>();
        }
    }
}
