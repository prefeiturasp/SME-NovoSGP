using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterResumoProfessorPorRFAnoLetivoQueryHandler : IRequestHandler<ObterResumoProfessorPorRFAnoLetivoQuery, ProfessorResumoDto>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterResumoProfessorPorRFAnoLetivoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<ProfessorResumoDto> Handle(ObterResumoProfessorPorRFAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");

            var resposta = await httpClient.GetAsync($"professores/{request.CodigoRF}/BuscarPorRf/{request.AnoLetivo}?buscarOutrosCargos={request.BuscarOutrosCargos}");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProfessorResumoDto>(json);
            }
            else
            {
                throw new Exception($"Não foi possível obter o resumo do professor rf : {request.CodigoRF} no ano letivo {request.AnoLetivo}.");
            }
        }
    }
}
