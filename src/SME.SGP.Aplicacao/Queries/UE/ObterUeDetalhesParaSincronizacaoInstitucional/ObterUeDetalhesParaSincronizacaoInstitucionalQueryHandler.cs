using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUeDetalhesParaSincronizacaoInstitucionalQueryHandler : IRequestHandler<ObterUeDetalhesParaSincronizacaoInstitucionalQuery, UeDetalhesParaSincronizacaoInstituicionalDto>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterUeDetalhesParaSincronizacaoInstitucionalQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<UeDetalhesParaSincronizacaoInstituicionalDto> Handle(ObterUeDetalhesParaSincronizacaoInstitucionalQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"escolas/{request.UeCodigo}/sincronizacoes-institucionais");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UeDetalhesParaSincronizacaoInstituicionalDto>(json);
            }

            return default;
        }
    }
}
