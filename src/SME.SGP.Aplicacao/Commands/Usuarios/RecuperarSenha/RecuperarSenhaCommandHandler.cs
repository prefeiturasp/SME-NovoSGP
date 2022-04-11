using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RecuperarSenhaCommandHandler : IRequestHandler<RecuperarSenhaCommand, string>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public RecuperarSenhaCommandHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<string> Handle(RecuperarSenhaCommand request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"autenticacao/recuperar/senha/usuario/{request.Login}");

            if (!resposta.IsSuccessStatusCode)
                await RegistraLogErro(resposta, request.Login);

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<string>(json);
        }

        private async Task RegistraLogErro(HttpResponseMessage resposta, string login)
        {
            var mensagem = await resposta.Content.ReadAsStringAsync();
            await mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro ao Recuperar Senha no EOL, código de erro: {resposta.StatusCode}, mensagem: {mensagem ?? "Sem mensagem"}, Usuario:{login}, Request: {JsonConvert.SerializeObject(resposta.RequestMessage)}, ", LogNivel.Critico, LogContexto.ApiEol, string.Empty));
        }
    }
}
