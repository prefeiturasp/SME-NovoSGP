using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSupervisoresPorDreEolQueryHandler : IRequestHandler<ObterSupervisoresPorDreEolQuery, IEnumerable<SupervisoresRetornoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterSupervisoresPorDreEolQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<SupervisoresRetornoDto>> Handle(ObterSupervisoresPorDreEolQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_DRES_SUPERVISORES, request.DreCodigo), cancellationToken);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
                return JsonConvert.DeserializeObject<IEnumerable<SupervisoresRetornoDto>>(json);
            }

            string erro = $"Não foi possível consultar os supervisores no EOL - HttpCode {(int)resposta.StatusCode} - Dre:{request.DreCodigo} - erro : {resposta.Content.ReadAsStringAsync()}";
            throw new NegocioException(erro);
        }
    }
}
