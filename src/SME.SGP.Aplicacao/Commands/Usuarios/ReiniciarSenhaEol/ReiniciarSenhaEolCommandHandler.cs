using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReiniciarSenhaEolCommandHandler : IRequestHandler<ReiniciarSenhaEolCommand>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ReiniciarSenhaEolCommandHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task Handle(ReiniciarSenhaEolCommand request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            
            var valoresParaEnvio = new List<KeyValuePair<string, string>> { { new ("login", request.Login) }};
            
            var resposta = await httpClient.PostAsync(ServicosEolConstants.URL_AUTENTICACAO_SGP_REINICIAR_SENHA, new FormUrlEncodedContent(valoresParaEnvio));

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Não foi possível reiniciar a senha deste usuário");
        }
    }
}
