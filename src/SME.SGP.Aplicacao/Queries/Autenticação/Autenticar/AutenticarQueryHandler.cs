using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AutenticarQueryHandler : IRequestHandler<AutenticarQuery, AutenticacaoApiEolDto>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public AutenticarQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<AutenticacaoApiEolDto> Handle(AutenticarQuery request, CancellationToken cancellationToken)
        {
            var parametros = JsonConvert.SerializeObject(new { request.Login, request.Senha });
            
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            
            var resposta = await httpClient.PostAsync(ServicosEolConstants.URL_AUTENTICACAO,new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode)
                return default; //Aqui antes estava retornando null

            var json = await resposta.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<AutenticacaoApiEolDto>(json);
        }
    }
}
