﻿using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Net.Http;
using System.Text;
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

            var parametros = JsonConvert.SerializeObject(request.Login);
            var resposta = await httpClient.PostAsync($"v1/autenticacao/RecuperarSenha/usuario?sistema=1", new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));
            
            if (!resposta.IsSuccessStatusCode)
                await RegistraLogErro(resposta, request.Login);

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<string>(json);
        }

        private async Task RegistraLogErro(HttpResponseMessage resposta, string login)
        {
            var mensagem = await resposta.Content.ReadAsStringAsync();
            var titulo = "Ocorreu um erro ao Solicitar Recuperação de Senha no EOL";
            await mediator.Send(new SalvarLogViaRabbitCommand(titulo,
                                                              LogNivel.Critico,
                                                              LogContexto.ApiEol,
                                                              $"código de erro: {resposta.StatusCode}, mensagem: {mensagem ?? "Sem mensagem"}, Usuario:{login}, Request: {JsonConvert.SerializeObject(resposta.RequestMessage)}"));

            throw new Exception(titulo);
        }
    }
}
