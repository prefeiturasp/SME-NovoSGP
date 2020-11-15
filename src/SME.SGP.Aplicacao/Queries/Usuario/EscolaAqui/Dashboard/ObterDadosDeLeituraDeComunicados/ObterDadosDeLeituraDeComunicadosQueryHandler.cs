using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Usuario.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicados
{
    public class ObterDadosDeLeituraDeComunicadosQueryHandler : IRequestHandler<ObterDadosDeLeituraDeComunicadosQuery, IEnumerable<DadosDeLeituraDoComunicadoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private const string BaseUrl = "/api/v1/dashboard/leitura";

        public ObterDadosDeLeituraDeComunicadosQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<DadosDeLeituraDoComunicadoDto>> Handle(ObterDadosDeLeituraDeComunicadosQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoAcompanhamentoEscolar");
            var url = new StringBuilder(BaseUrl);

            url.Append(@"?coumnicadoId=" + request.ComunicadoId);
            if (!string.IsNullOrEmpty(request.CodigoDre))
            {
                url.Append(@"&codigoDre=" + request.CodigoDre);

                if(!string.IsNullOrEmpty(request.CodigoUe))
                    url.Append(@"&codigoUe=" + request.CodigoUe);
            }

            var resposta = await httpClient.GetAsync($"{url}", cancellationToken);
            if (!resposta.IsSuccessStatusCode || resposta.StatusCode == HttpStatusCode.NoContent)
                throw new Exception("Não foi possível obter dados de de leitura de comunicados pelo aplicativo.");

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<DadosDeLeituraDoComunicadoDto>>(json);
        }
    }
}