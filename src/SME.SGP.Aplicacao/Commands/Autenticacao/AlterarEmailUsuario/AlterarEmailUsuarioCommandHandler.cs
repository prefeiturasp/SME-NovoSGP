using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarEmailUsuarioCommandHandler : AsyncRequestHandler<AlterarEmailUsuarioCommand>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public AlterarEmailUsuarioCommandHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        protected override async Task Handle(AlterarEmailUsuarioCommand request, CancellationToken cancellationToken)
        {
            var valoresParaEnvio = new List<KeyValuePair<string, string>> {
                { new ("usuario", request.Login) },
                { new ("email", request.Email) }};

            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.PostAsync(ServicosEolConstants.URL_AUTENTICACAO_SGP_ALTERAR_EMAIL, new FormUrlEncodedContent(valoresParaEnvio));

            if (resposta.IsSuccessStatusCode)
                return;

            var mensagem = await resposta.Content.ReadAsStringAsync();

            throw new NegocioException(mensagem);
        }
    }
}
