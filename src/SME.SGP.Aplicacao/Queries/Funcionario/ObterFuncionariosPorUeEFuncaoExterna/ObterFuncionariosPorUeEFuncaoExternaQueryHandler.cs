using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    internal class ObterFuncionariosPorUeEFuncaoExternaQueryHandler : IRequestHandler<ObterFuncionariosPorUeEFuncaoExternaQuery, IEnumerable<FuncionarioDTO>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterFuncionariosPorUeEFuncaoExternaQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<IEnumerable<FuncionarioDTO>> Handle(ObterFuncionariosPorUeEFuncaoExternaQuery request, CancellationToken cancellationToken)
        {
            var listaRetorno = Enumerable.Empty<FuncionarioDTO>();
            using (var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO))
            {
                var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_ESCOLAS_FUNCIONARIOS_FUNCOES_EXTERNAS, request.CodigoUE, request.CodigoFuncaoExterna));
                if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    listaRetorno = JsonConvert.DeserializeObject<IEnumerable<FuncionarioDTO>>(json) as List<FuncionarioDTO>;
                }

            }
            return listaRetorno;
        }

    }
}