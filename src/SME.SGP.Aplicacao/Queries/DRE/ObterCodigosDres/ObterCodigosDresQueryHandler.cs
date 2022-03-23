using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosDresQueryHandler : IRequestHandler<ObterCodigosDresQuery, string[]>
    {

        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;
        private const string BaseUrl = "abrangencia/codigos-dres";

        public ObterCodigosDresQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
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
                await mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro na tentativa de buscar os codigos das Dres no EOL - HttpCode {resposta.StatusCode} - Body {resposta.Content?.ReadAsStringAsync()?.Result ?? string.Empty} - URL: {httpClient.BaseAddress}", LogNivel.Negocio, LogContexto.Relatorios, string.Empty));

                throw new NegocioException($"Erro ao obter os códigos de DREs no EOL. URL base: {httpClient.BaseAddress}");
            }
        }
    }
}