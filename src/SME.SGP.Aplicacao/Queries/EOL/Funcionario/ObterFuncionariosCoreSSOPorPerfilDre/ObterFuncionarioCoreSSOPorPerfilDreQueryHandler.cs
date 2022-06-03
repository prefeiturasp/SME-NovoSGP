using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    internal class ObterFuncionarioCoreSSOPorPerfilDreQueryHandler : IRequestHandler<ObterFuncionarioCoreSSOPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterFuncionarioCoreSSOPorPerfilDreQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionarioCoreSSOPorPerfilDreQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.PostAsync($@"funcionarios/unidade/{request.CodigoDre}", new StringContent(JsonConvert.SerializeObject(request.CodigoPerfil), Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode || resposta.StatusCode == HttpStatusCode.NoContent)
                return null;

            var json = await resposta.Content.ReadAsStringAsync();

            return await Task.FromResult(JsonConvert.DeserializeObject<IEnumerable<UsuarioEolRetornoDto>>(json));
        }
    }
}
