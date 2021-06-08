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
    public class ObterFuncionariosCompletosPorUeECargoQueryHandler : IRequestHandler<ObterFuncionariosCompletosPorUeECargoQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterFuncionariosCompletosPorUeECargoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionariosCompletosPorUeECargoQuery request, CancellationToken cancellationToken)
        {
            var listaRetorno = new List<UsuarioEolRetornoDto>();

            using (var httpClient = httpClientFactory.CreateClient("servicoEOL"))
            {
                var resposta = await httpClient.GetAsync($"/api/escolas/{request.UeCodigo}/funcionarios/cargos/{request.CargoCodigo}");

                if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    var listaRetornoEOL = JsonConvert.DeserializeObject<IEnumerable<UsuarioEolRetornoDto>>(json) as List<UsuarioEolRetornoDto>;
                    if (listaRetornoEOL.Any())
                        listaRetorno.AddRange(listaRetornoEOL);
                }

            }

            return listaRetorno;
        }
    }
}
