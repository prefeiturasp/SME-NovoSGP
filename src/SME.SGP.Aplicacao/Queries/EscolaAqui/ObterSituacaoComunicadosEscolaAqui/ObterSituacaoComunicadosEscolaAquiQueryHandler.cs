using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.EscolaAqui.ComunicadoEvento;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSituacaoComunicadosEscolaAquiQueryHandler : IRequestHandler<ObterSituacaoComunicadosEscolaAquiQuery, IEnumerable<SituacaoComunicadoEADto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private const string BaseUrl = "/api/v1/UsuarioNotificacaoLeitura/status-leitura";

        public ObterSituacaoComunicadosEscolaAquiQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory)); ;
        }

        public async Task<IEnumerable<SituacaoComunicadoEADto>> Handle(ObterSituacaoComunicadosEscolaAquiQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoAcompanhamentoEscolar");
            var url = new StringBuilder(BaseUrl);

            url.Append(@"?codigoAluno=" + Convert.ToInt64(request.AlunoCodigo));

            foreach (var comunicadoId in request.ComunicadosIds)
            {
                url.Append(@"&notificacaoId=" + comunicadoId);
            }

            var resposta = await httpClient.GetAsync($"{url}", cancellationToken);
            if (!resposta.IsSuccessStatusCode || resposta.StatusCode == HttpStatusCode.NoContent)
                throw new NegocioException("Não foram encontrados dados de leitura de comunicados.", HttpStatusCode.InternalServerError);

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<SituacaoComunicadoEADto>>(json);
        }
    }
}