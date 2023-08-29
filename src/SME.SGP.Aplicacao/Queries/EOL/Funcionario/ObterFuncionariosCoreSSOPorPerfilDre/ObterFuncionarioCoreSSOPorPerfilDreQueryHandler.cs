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
    public class ObterFuncionarioCoreSSOPorPerfilDreQueryHandler : IRequestHandler<ObterFuncionarioCoreSSOPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterFuncionarioCoreSSOPorPerfilDreQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionarioCoreSSOPorPerfilDreQuery request, CancellationToken cancellationToken)
        {
            var json = new StringContent(JsonConvert.SerializeObject(new string[] { request.CodigoPerfil.ToString() }), Encoding.UTF8, "application/json");

            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var resposta = await httpClient.PostAsync(string.Format(ServicosEolConstants.URL_FUNCIONARIOS_UNIDADE, request.CodigoDre), json);

            if (!resposta.IsSuccessStatusCode || resposta.StatusCode == HttpStatusCode.NoContent)
                return null;

            var jsonRetorno = await resposta.Content.ReadAsStringAsync();

            return await Task.FromResult(JsonConvert.DeserializeObject<IEnumerable<UsuarioEolRetornoDto>>(jsonRetorno));
        }
    }
}
