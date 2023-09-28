using MediatR;
using Newtonsoft.Json;
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
    public class ObterFuncionariosPorRfUeNomeServidorQueryHandler : IRequestHandler<ObterFuncionariosPorRfUeNomeServidorQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterFuncionariosPorRfUeNomeServidorQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionariosPorRfUeNomeServidorQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var buscaFuncionariosFiltroDto = new BuscaFuncionariosFiltroDto()
            {
                NomeServidor = request.NomeServidor,
                CodigoRF = request.CodigoRF,
                CodigoUE = request.CodigoUE,
            };
            
            var jsonParaPost = new StringContent(JsonConvert.SerializeObject(buscaFuncionariosFiltroDto), UnicodeEncoding.UTF8, "application/json");

            var resposta = await httpClient.PostAsync(ServicosEolConstants.URL_FUNCIONARIOS, jsonParaPost);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<UsuarioEolRetornoDto>>(json);
            }

            return default;
        }
    }
}
