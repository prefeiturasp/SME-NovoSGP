using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra.Dtos.EscolaAqui.DashboardAdesao;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotaisAdesaoAgrupadosPorDreQueryHandler : IRequestHandler<ObterTotaisAdesaoAgrupadosPorDreQuery, IEnumerable<TotaisAdesaoAgrupadoProDreResultado>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterTotaisAdesaoAgrupadosPorDreQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<TotaisAdesaoAgrupadoProDreResultado>> Handle(ObterTotaisAdesaoAgrupadosPorDreQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoAcompanhamentoEscolar");
            var url = new StringBuilder(@"/api/v1/dashboard/adesao/agrupados");
            var resposta = await httpClient.GetAsync($"{url}");

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<TotaisAdesaoAgrupadoProDreResultado>>(json);
            }
            
            throw new Exception("Não foi possível obter dados de adesão do aplicativo.");
        }
    }
}