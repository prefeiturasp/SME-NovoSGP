using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorCargoUeQueryHandler : IRequestHandler<ObterFuncionariosPorCargoUeQuery,IEnumerable<UsuarioEolRetornoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterFuncionariosPorCargoUeQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionariosPorCargoUeQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"escolas/{request.UeId}/funcionarios/cargos/{request.CargoId}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<UsuarioEolRetornoDto>>(json);
            }
            return Enumerable.Empty<UsuarioEolRetornoDto>();
        }
    }
}