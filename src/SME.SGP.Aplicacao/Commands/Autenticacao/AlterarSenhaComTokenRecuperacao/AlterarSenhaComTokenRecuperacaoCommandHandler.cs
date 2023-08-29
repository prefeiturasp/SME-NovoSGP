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
    public class AlterarSenhaComTokenRecuperacaoCommandHandler : IRequestHandler<AlterarSenhaComTokenRecuperacaoCommand, string>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public AlterarSenhaComTokenRecuperacaoCommandHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<string> Handle(AlterarSenhaComTokenRecuperacaoCommand request, CancellationToken cancellationToken)
        {
            var valoresParaEnvio = new List<KeyValuePair<string, string>> {
                { new KeyValuePair<string, string>("token", request.Token.ToString()) },
                { new KeyValuePair<string, string>("senha", request.Senha) }};

            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.PostAsync(ServicosEolConstants.URL_AUTENTICACAO_ALTERAR_SENHA, new FormUrlEncodedContent(valoresParaEnvio));

            if (!resposta.IsSuccessStatusCode)
                await RegistraLogErro(resposta, request.Token);

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<string>(json);
        }

        private async Task RegistraLogErro(HttpResponseMessage resposta, Guid token)
        {
            var mensagem = await resposta.Content.ReadAsStringAsync();
            var titulo = "Ocorreu um erro ao Alterar Senha no EOL";
            await mediator.Send(new SalvarLogViaRabbitCommand(titulo,
                                                              LogNivel.Critico,
                                                              LogContexto.ApiEol,
                                                              $"código de erro: {resposta.StatusCode}, mensagem: {mensagem ?? "Sem mensagem"}, Token:{token}, Request: {JsonConvert.SerializeObject(resposta.RequestMessage)}"));

            throw new NegocioException(mensagem ?? titulo);
        }
    }
}
