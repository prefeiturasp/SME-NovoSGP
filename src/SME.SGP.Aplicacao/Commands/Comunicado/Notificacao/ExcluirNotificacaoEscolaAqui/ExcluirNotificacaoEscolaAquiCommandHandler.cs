using MediatR;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoEscolaAquiCommandHandler : IRequestHandler<ExcluirNotificacaoEscolaAquiCommand, bool>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ExcluirNotificacaoEscolaAquiCommandHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<bool> Handle(ExcluirNotificacaoEscolaAquiCommand request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoAcompanhamentoEscolar");

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress.ToString() + "v1/notificacao"),
                Method = HttpMethod.Delete,
                Content = new StringContent(JsonConvert.SerializeObject(request.Ids), Encoding.UTF8, "application/json"),
            };           

            var resposta = await httpClient.SendAsync(requestMessage);

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
                return true;
            else
                throw new Exception($"Não foi possível excluir os dados no aplicativo do aluno");
        }
    }
}
