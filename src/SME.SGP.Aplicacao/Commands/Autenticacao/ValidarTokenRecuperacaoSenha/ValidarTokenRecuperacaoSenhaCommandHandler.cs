using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ValidarTokenRecuperacaoSenhaCommandHandler : IRequestHandler<ValidarTokenRecuperacaoSenhaCommand, bool>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ValidarTokenRecuperacaoSenhaCommandHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ValidarTokenRecuperacaoSenhaCommand request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var parametros = JsonConvert.SerializeObject(request.Token);
            var resposta = await httpClient.PostAsync(ServicosEolConstants.URL_AUTENTICACAO_RECUPERACAO_SENHA_TOKEN_VALIDAR, new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode)
                await RegistraLogErro(resposta, request.Token);

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(json);
        }

        private async Task RegistraLogErro(HttpResponseMessage resposta, Guid token)
        {
            var mensagem = await resposta.Content.ReadAsStringAsync();
            var titulo = "Ocorreu um erro ao validar token de recuperação de senha no EOL";
            await mediator.Send(new SalvarLogViaRabbitCommand(titulo,
                                                              LogNivel.Critico,
                                                              LogContexto.ApiEol,
                                                              $"código de erro: {resposta.StatusCode}, mensagem: {mensagem ?? "Sem mensagem"}, Token:{token}, Request: {JsonConvert.SerializeObject(resposta.RequestMessage)}"));

            throw new NegocioException(mensagem ?? titulo);
        }
    }
}
