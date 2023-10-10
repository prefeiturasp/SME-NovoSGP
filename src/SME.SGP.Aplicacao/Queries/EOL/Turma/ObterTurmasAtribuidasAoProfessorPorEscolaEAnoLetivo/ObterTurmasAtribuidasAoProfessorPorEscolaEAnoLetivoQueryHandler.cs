using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivoQueryHandler : IRequestHandler<ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivoQuery, IEnumerable<TurmaDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivoQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<TurmaDto>> Handle(ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var url = string.Format(ServicosEolConstants.URL_PROFESSORES_ESCOLAS_TURMAS_ANOS_LETIVOS, request.RfProfessor, request.CodigoEscola, request.AnoLetivo);
            
            var resposta = await httpClient.GetAsync(url);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<TurmaDto>>(json);
            }
            return default;
        }
    }
}
