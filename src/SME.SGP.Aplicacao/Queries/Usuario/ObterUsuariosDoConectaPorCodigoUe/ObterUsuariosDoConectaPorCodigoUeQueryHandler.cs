using MediatR;
using Microsoft.Extensions.Logging;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Usuario.ObterUsuariosDoConectaPorCodigoUe
{
    public class ObterUsuariosDoConectaPorCodigoUeQueryHandler : IRequestHandler<ObterUsuariosDoConectaPorCodigoUeQuery, IEnumerable<DadosLoginUsuarioConectaDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;
        private readonly ILogger<ObterUsuariosDoConectaPorCodigoUeQueryHandler> logger;

        public ObterUsuariosDoConectaPorCodigoUeQueryHandler(IHttpClientFactory httpClientFactory,
                                                            IMediator mediator,
                                                            ILogger<ObterUsuariosDoConectaPorCodigoUeQueryHandler> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.mediator = mediator;
            this.logger = logger;
        }

        public async Task<IEnumerable<DadosLoginUsuarioConectaDto>> Handle(ObterUsuariosDoConectaPorCodigoUeQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicoConectaFormacaoConstants.Servico);
            var url = string.Format(ServicoConectaFormacaoConstants.URL_OBTER_USUARIOS_POR_CODIGO_UE, request.CodigoUe);

            if (!string.IsNullOrEmpty(request.Nome) || !string.IsNullOrEmpty(request.Login))
                url += "?";

            if (!string.IsNullOrEmpty(request.Nome))
                url += $"nome={request.Nome}&";

            if (!string.IsNullOrEmpty(request.Login))
                url += $"login={request.Login}&";

            var resposta = await httpClient.GetAsync(url);
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<DadosLoginUsuarioConectaDto>>(json);
            }
            logger.LogError($"Ocorreu um erro ao obter usuários do Conecta por código UE {request.CodigoUe}, código de erro: {resposta.StatusCode}, mensagem: {await resposta.Content.ReadAsStringAsync()}");
            await mediator.Send(new SalvarLogViaRabbitCommand(
                $"Ocorreu um erro ao obter usuários do Conecta por código UE {request.CodigoUe}, código de erro: {resposta.StatusCode}",
                LogNivel.Critico,
                LogContexto.ApiConectaFormacao,
                $"Mensagem: {await resposta.Content.ReadAsStringAsync()}, Request: {Newtonsoft.Json.JsonConvert.SerializeObject(resposta.RequestMessage)}"));
            return new List<DadosLoginUsuarioConectaDto>();
        }
    }
}
