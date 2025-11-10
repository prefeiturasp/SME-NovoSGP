using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Escola;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosEscolaPorUeEolQueryHandler : IRequestHandler<ObterDadosEscolaPorUeEolQuery, DadosEscolaDto>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterDadosEscolaPorUeEolQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<DadosEscolaDto> Handle(ObterDadosEscolaPorUeEolQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            try
            {
                var resposta = await httpClient
                    .GetAsync(string.Format(ServicosEolConstants.URL_ESCOLAS_DADOS, request.UeCodigo), cancellationToken);

                if (resposta.IsSuccessStatusCode)
                {
                    var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
                    return JsonConvert.DeserializeObject<DadosEscolaDto>(json);
                }
                else
                {
                    throw new HttpRequestException($"Erro ao obter dados da escola. Código de status: {resposta.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao processar a requisição para obter dados da escola: {ex.Message}", ex);
            }
        }
    }
}
