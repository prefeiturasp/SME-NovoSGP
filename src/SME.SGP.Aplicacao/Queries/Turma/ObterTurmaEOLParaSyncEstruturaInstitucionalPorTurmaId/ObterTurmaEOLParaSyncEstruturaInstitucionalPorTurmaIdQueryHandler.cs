using MediatR;
using Newtonsoft.Json;
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

        public ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<TurmaParaSyncInstitucionalDto> Handle(ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"ues/{request.UeCodigo}/turmas/{request.TurmaId}/sincronizacoes-institucionais");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TurmaParaSyncInstitucionalDto>(json);
            }
            return default;
        }
    }
}
