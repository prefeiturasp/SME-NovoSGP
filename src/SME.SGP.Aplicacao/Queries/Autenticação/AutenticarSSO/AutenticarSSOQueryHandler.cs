using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra.Dtos;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Autenticação.AutenticarSSO
{
    public class AutenticarSSOQueryHandler : IRequestHandler<AutenticarSSOQuery, AutenticacaoSSODto>
    {
        public async Task<AutenticacaoSSODto> Handle(AutenticarSSOQuery request, CancellationToken cancellationToken)
        {
            var parametros = JsonConvert.SerializeObject(new { request.Login, request.Senha });

            using var httpClient = new HttpClient() { BaseAddress = new Uri("") };
            var resposta = await httpClient.PostAsync("", new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"), cancellationToken);

            if (!resposta.IsSuccessStatusCode)
                return default;

            var json = await resposta.Content.ReadAsStringAsync(cancellationToken);

            return JsonConvert.DeserializeObject<AutenticacaoSSODto>(json);
        }
    }
}
