using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCepQueryHandler : IRequestHandler<ObterCepQuery, CepDto>
    {
        private readonly HttpClient httpClient;

        public ObterCepQueryHandler(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<CepDto> Handle(ObterCepQuery request, CancellationToken cancellationToken)
        {
            var resposta = await httpClient.GetAsync($"https://opencep.com/v1/{request.Cep}");

            if (!resposta.IsSuccessStatusCode) return default;

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CepDto>(json);
        }
    }
}
