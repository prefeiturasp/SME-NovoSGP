using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterMeusDadosQueryHandler : IRequestHandler<ObterMeusDadosQuery, MeusDadosDto>
    {
        private readonly HttpClient httpClient;
        private readonly IServicoLog servicoLog;
        
        public ObterMeusDadosQueryHandler(HttpClient httpClient, IServicoLog servicoLog)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<MeusDadosDto> Handle(ObterMeusDadosQuery request, CancellationToken cancellationToken)
        {
            var retorno = new MeusDadosDto();
            var resposta = await httpClient.GetAsync($"AutenticacaoSgp/{request.Login}/dados");
            var jsonString = await resposta.Content.ReadAsStringAsync();
            if (!resposta.IsSuccessStatusCode)
            {
                await RegistrarLogSentryAsync(resposta, "ObterMeusDados", "login = " + request.Login);
                throw new NegocioException("Não foi possível obter os dados do usuário");
            }
            retorno = JsonConvert.DeserializeObject<MeusDadosDto>(jsonString);
            return retorno;
        }

        private async Task RegistrarLogSentryAsync(HttpResponseMessage resposta, string rotina, string parametros)
        {
            if (resposta.StatusCode != HttpStatusCode.NotFound)
            {
                var mensagem = await resposta.Content.ReadAsStringAsync();
                servicoLog.Registrar(new NegocioException($"Ocorreu um erro ao {rotina} no EOL, código de erro: {resposta.StatusCode}, mensagem: {mensagem ?? "Sem mensagem"},Parametros:{parametros}, Request: {JsonConvert.SerializeObject(resposta.RequestMessage)}, "));
            }
        }
    }
}