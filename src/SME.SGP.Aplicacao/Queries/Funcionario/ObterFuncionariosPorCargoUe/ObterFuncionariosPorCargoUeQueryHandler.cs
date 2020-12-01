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
    public class ObterFuncionariosPorCargoUeQueryHandler : IRequestHandler<ObterFuncionariosPorCargoUeQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterFuncionariosPorCargoUeQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionariosPorCargoUeQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");

            //var resposta = await httpClient.GetAsync($"/api/funcionarios/cargos/{(int)request.Cargo}");
            
            var resposta = await httpClient.GetAsync($"/api/escolas/{request.CodigoUe}/funcionarios/cargos/{request.Cargo}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<UsuarioEolRetornoDto>>(json);
            }
            return Enumerable.Empty<UsuarioEolRetornoDto>();

            //if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            //{
            //    var json = await resposta.Content.ReadAsStringAsync();
            //    return JsonConvert.DeserializeObject<IEnumerable<FuncionarioDto>>(json);
            //}

            //return Enumerable.Empty<FuncionarioDto>();
        }
    }
}
