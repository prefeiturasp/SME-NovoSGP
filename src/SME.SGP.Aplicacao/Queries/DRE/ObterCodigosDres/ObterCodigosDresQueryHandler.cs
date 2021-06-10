using MediatR;
using Newtonsoft.Json;
using Sentry;
using SME.SGP.Dominio;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosDresQueryHandler : IRequestHandler<ObterCodigosDresQuery, string[]>
    {

        private readonly IHttpClientFactory httpClientFactory;
        private const string BaseUrl = "abrangencia/codigos-dres";

        public ObterCodigosDresQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<string[]> Handle(ObterCodigosDresQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var url = new StringBuilder(BaseUrl);

            var resposta = await httpClient.GetAsync($"{url}", cancellationToken);

            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<string[]>(json);
            }
            else
            {
                SentrySdk.AddBreadcrumb($"Ocorreu um erro na tentativa de buscar os codigos das Dres no EOL - HttpCode {resposta.StatusCode} - Body {resposta.Content?.ReadAsStringAsync()?.Result ?? string.Empty} - URL: {httpClient.BaseAddress}");
                throw new NegocioException($"Erro ao obter os códigos de DREs no EOL. URL base: {httpClient.BaseAddress}");
            }
        }
    }
}