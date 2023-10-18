using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAutenticacaoSemSenhaQueryHandler : IRequestHandler<ObterAutenticacaoSemSenhaQuery, AutenticacaoApiEolDto>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterAutenticacaoSemSenhaQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<AutenticacaoApiEolDto> Handle(ObterAutenticacaoSemSenhaQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            
            var url = string.Format(ServicosEolConstants.URL_AUTENTICACAO_AUTENTICAR_SEM_SENHA,request.Login);

            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException($"Não foram encontrados dados do usuário {request.Login}");

            var json = await resposta.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<AutenticacaoApiEolDto>(json);
        }
    }
}
