using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.EscolaAqui;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPorCpfQueryHandler : IRequestHandler<ObterUsuarioPorCpfQuery, UsuarioEscolaAquiDto>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterUsuarioPorCpfQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<UsuarioEscolaAquiDto> Handle(ObterUsuarioPorCpfQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoAcompanhamentoEscolar");

            var resposta = await httpClient.GetAsync($"/api/v1/usuario/dre/{request.CodigoDre}/ue/{request.CodigoUe}/cpf/{request.Cpf}");

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UsuarioEscolaAquiDto>(json);
            }
            else
            {
                var json = await resposta.Content.ReadAsStringAsync();
                var respostaApi = JsonConvert.DeserializeObject<RespostaApi>(json);
                throw new NegocioException(respostaApi.Erros[0].ToString());
            }
        }
    }
}
