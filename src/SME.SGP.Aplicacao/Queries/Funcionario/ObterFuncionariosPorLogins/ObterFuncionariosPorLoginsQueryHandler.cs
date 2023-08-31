using MediatR;
using Newtonsoft.Json;
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
    public class ObterFuncionariosPorLoginsQueryHandler : IRequestHandler<ObterFuncionariosPorLoginsQuery, IEnumerable<FuncionarioUnidadeDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterFuncionariosPorLoginsQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<FuncionarioUnidadeDto>> Handle(ObterFuncionariosPorLoginsQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var resposta = await httpClient.PostAsync(ServicosEolConstants.URL_FUNCIONARIOS_BUSCAR_LISTA_LOGIN, new StringContent(JsonConvert.SerializeObject(request.Logins),
                Encoding.UTF8, "application/json-patch+json"), cancellationToken);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
                return JsonConvert.DeserializeObject<IEnumerable<FuncionarioUnidadeDto>>(json);
            }

            throw new Exception($"Não foi possível localizar os logins: {string.Join(",", request.Logins)}.");
        }
    }
}
