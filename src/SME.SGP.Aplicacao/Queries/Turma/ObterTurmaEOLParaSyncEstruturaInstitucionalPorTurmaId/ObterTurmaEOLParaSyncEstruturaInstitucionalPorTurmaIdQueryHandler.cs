using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQueryHandler : IRequestHandler<ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery, TurmaParaSyncInstitucionalDto>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<TurmaParaSyncInstitucionalDto> Handle(ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_UES_TURMAS_SINCRONIZACOES, request.UeCodigo, request.TurmaId));
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TurmaParaSyncInstitucionalDto>(json);
            }
            return default;
        }
    }
}
