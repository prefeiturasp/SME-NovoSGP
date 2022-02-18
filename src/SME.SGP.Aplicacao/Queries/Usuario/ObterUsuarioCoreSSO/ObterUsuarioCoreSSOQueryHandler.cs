using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioCoreSSOQueryHandler : IRequestHandler<ObterUsuarioCoreSSOQuery, MeusDadosDto>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterUsuarioCoreSSOQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<MeusDadosDto> Handle(ObterUsuarioCoreSSOQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");

            var url = $"AutenticacaoSgp/{request.CodigoRf}/dados";
            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Não foi possível obter os dados do usuário");

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MeusDadosDto>(json);
        }
    }
}