using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorCargoQueryHandler : IRequestHandler<ObterFuncionariosPorCargoQuery, IEnumerable<FuncionarioDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterFuncionariosPorCargoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<FuncionarioDto>> Handle(ObterFuncionariosPorCargoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");

            var resposta = await httpClient.GetAsync($"/api/funcionarios/cargos/{(int)request.Cargo}");

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<FuncionarioDto>>(json);
            }

            return Enumerable.Empty<FuncionarioDto>();
        }
    }
}
