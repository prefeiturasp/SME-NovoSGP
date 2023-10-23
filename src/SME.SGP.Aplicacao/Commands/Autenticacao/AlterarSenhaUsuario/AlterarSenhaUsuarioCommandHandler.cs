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
    public class AlterarSenhaUsuarioCommandHandler : IRequestHandler<AlterarSenhaUsuarioCommand,AlterarSenhaRespostaDto>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public AlterarSenhaUsuarioCommandHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<AlterarSenhaRespostaDto> Handle(AlterarSenhaUsuarioCommand request, CancellationToken cancellationToken)
        {
            var valoresParaEnvio = new List<KeyValuePair<string, string>> { new ("usuario", request.Login), new ("senha", request.Senha)};

            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            
            var resposta = await httpClient.PostAsync(ServicosEolConstants.URL_AUTENTICACAO_SGP_ALTERAR_SENHA, new FormUrlEncodedContent(valoresParaEnvio));

            return new AlterarSenhaRespostaDto
            {
                Mensagem = resposta.IsSuccessStatusCode ? "" : await resposta.Content.ReadAsStringAsync(),
                StatusRetorno = (int)resposta.StatusCode,
                SenhaAlterada = resposta.IsSuccessStatusCode
            };
        }
    }
}
