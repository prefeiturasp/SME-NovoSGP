using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.EscolaAqui;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SolicitarReiniciarSenhaCommandHandler : IRequestHandler<SolicitarReiniciarSenhaCommand, RespostaSolicitarReiniciarSenhaDto>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public SolicitarReiniciarSenhaCommandHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<RespostaSolicitarReiniciarSenhaDto> Handle(SolicitarReiniciarSenhaCommand request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoAcompanhamentoEscolar");

            var solicitarReiniciarSenhaDto = new SolicitarReiniciarSenhaDto(request.Cpf);
            var parametros = JsonConvert.SerializeObject(solicitarReiniciarSenhaDto);
            var resposta = await httpClient.PutAsync($"/api/v1/Autenticacao/Senha/ReiniciarSenha", new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));
            
            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                var respostaApi = JsonConvert.DeserializeObject<RespostaApi>(json);

                return new RespostaSolicitarReiniciarSenhaDto(respostaApi.Data.ToString());
            }
            else
            {
                var json = await resposta.Content.ReadAsStringAsync();
                var respostaApi = JsonConvert.DeserializeObject<RespostaApi>(json);
                throw new NegocioException(respostaApi.Erros[0].ToString(), HttpStatusCode.BadRequest);
            }
        }
    }

}
