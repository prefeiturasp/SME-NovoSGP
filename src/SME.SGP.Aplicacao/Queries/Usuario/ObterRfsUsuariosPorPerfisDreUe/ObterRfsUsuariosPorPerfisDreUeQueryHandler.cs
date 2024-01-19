using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;

namespace SME.SGP.Aplicacao
{
    public class ObterRfsUsuariosPorPerfisDreUeQueryHandler : IRequestHandler<ObterRfsUsuariosPorPerfisDreUeQuery, string[]>
    {
        private readonly IMediator mediator;
        private readonly IHttpClientFactory httpClientFactory;

        public ObterRfsUsuariosPorPerfisDreUeQueryHandler(IMediator mediator,
                                                              IHttpClientFactory httpClientFactory)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<string[]> Handle(ObterRfsUsuariosPorPerfisDreUeQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var parametros = JsonConvert.SerializeObject(request);

            var resposta = await httpClient.PostAsync(ServicosEolConstants.URL_ABRANGENCIAS_PERFIS_USUARIO,
                                                    new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));
            if (!resposta.IsSuccessStatusCode)
            {
                var mensagem = await resposta.Content.ReadAsStringAsync();
                await mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro ao obter os usuários por perfil/ue/dre no EOL, código de erro: {resposta.StatusCode}, mensagem: {mensagem ?? "Sem mensagem"},Parametros:{parametros}, Request: {JsonConvert.SerializeObject(resposta.RequestMessage)}, ", LogNivel.Negocio, LogContexto.ApiEol, string.Empty));
                return new string[] { };
            }

            var json = resposta.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<string[]>(json);
        }

    }
}
