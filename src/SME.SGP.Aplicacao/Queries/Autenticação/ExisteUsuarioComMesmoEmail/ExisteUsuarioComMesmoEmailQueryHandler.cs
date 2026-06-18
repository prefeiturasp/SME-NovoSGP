using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExisteUsuarioComMesmoEmailQueryHandler : IRequestHandler<ExisteUsuarioComMesmoEmailQuery, bool>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ExisteUsuarioComMesmoEmailQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<bool> Handle(ExisteUsuarioComMesmoEmailQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_AUTENTICACAO_SGP_VALIDA_EMAIL_EXISTENTE, request.Login, request.Email));

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(json);
            }
            return false;
        }
    }
}
