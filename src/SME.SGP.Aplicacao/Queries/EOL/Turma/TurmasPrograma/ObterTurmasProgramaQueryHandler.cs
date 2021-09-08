using MediatR;
using Newtonsoft.Json;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasProgramaQueryHandler : IRequestHandler<ObterTurmasProgramaQuery, IEnumerable<string>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        public ObterTurmasProgramaQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<IEnumerable<string>> Handle(ObterTurmasProgramaQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var url = $"turmas/turmas-programa";
            try
            {
                var parametros = JsonConvert.SerializeObject(request.CodigosTurmas);
                var resposta = await httpClient.PostAsync(url, new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));
                if (resposta.IsSuccessStatusCode)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<string>>(json);
                }
                else
                {
                    string erro = $"Não foi possível obter as turmas de programa no EOL - HttpCode {(int)resposta.StatusCode}";
                    SentrySdk.AddBreadcrumb(erro);
                    return Enumerable.Empty<string>();
                }
            }
            catch (Exception e)
            {
                SentrySdk.CaptureMessage($"Erro ao obter as turmas de programa no EOL - Códigos:{String.Join(",", request.CodigosTurmas)} - Erro:{e.Message}");
                SentrySdk.CaptureException(e);
                throw e;
            }
        }
    }
}
