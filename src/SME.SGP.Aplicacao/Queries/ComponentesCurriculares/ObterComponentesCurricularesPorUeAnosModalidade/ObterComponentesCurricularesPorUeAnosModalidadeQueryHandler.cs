using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorUeAnosModalidadeQueryHandler : IRequestHandler<ObterComponentesCurricularesPorUeAnosModalidadeQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterComponentesCurricularesPorUeAnosModalidadeQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesPorUeAnosModalidadeQuery request, CancellationToken cancellationToken)
        {
            string[] turmasCodigo;

            if (request.TurmaCodigo == "-99")
            {
                //Obter tudo por anos\\
                //var codigoTurmasPorAnos = new mediat

                turmasCodigo = new string[1];
            }
            else turmasCodigo = new string[] { request.TurmaCodigo };

            var httpClient = httpClientFactory.CreateClient("servicoEOL");

            //var anos = String.Join("&anos=", request.Anos);
            var turmas = String.Join("&codigoTurmas=", turmasCodigo);

            var resposta = await httpClient.GetAsync($"/api/v1/componentes-curriculares/?id=0{turmas}");

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<ComponenteCurricularEol>>(json);
            }
            else throw new NegocioException("Não foi possível obter Componentes Curriculares.");
            
        }
            
    }
}
