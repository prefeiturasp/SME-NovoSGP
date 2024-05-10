using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorUeQueryHandler : IRequestHandler<ObterFuncionariosPorUeQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterFuncionariosPorUeQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionariosPorUeQuery request, CancellationToken cancellationToken)
        {
            var dto = new { CodigosRfs = request.CodigosRfs, Filtro = request.Filtro };
            var jsonParaPost = new StringContent(JsonConvert.SerializeObject(dto), UnicodeEncoding.UTF8, "application/json");
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.PostAsync(string.Format(ServicosEolConstants.URL_FUNCIONARIOS_UE, request.CodigoUe), jsonParaPost);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<UsuarioEolRetornoDto>>(json);
            }

            return null;
        }
    }
}
