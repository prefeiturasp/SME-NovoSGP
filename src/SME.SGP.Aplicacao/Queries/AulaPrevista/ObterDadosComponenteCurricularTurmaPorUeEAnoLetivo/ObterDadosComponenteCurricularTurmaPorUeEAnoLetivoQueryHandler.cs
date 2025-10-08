using MediatR;
using Newtonsoft.Json;
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
    public class ObterDadosComponenteCurricularTurmaPorUeEAnoLetivoQueryHandler : IRequestHandler<ObterDadosComponenteCurricularTurmaPorUeEAnoLetivoQuery, IEnumerable<DadosTurmaAulasAutomaticaDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        public ObterDadosComponenteCurricularTurmaPorUeEAnoLetivoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<DadosTurmaAulasAutomaticaDto>> Handle(ObterDadosComponenteCurricularTurmaPorUeEAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            var listaRetornoEOL = Enumerable.Empty<DadosTurmaAulasAutomaticaDto>();

            using (var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO))
            {
                var componentesCurriculares = String.Join("&componentesCurriculares=", request.ComponentesCurriculares);
                var resposta = await httpClient.GetAsync(ServicosEolConstants.URL_COMPONENTES_CURRICULARES_DADOS_AULA_TURMA + $"?AnoLetivo={request.AnoLetivo}&ueCodigo={request.UeCodigo}&componentesCurriculares={componentesCurriculares}&semestre={request.Semestre}", cancellationToken);

                if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    listaRetornoEOL = JsonConvert.DeserializeObject<IEnumerable<DadosTurmaAulasAutomaticaDto>>(json) as List<DadosTurmaAulasAutomaticaDto>;
                }
            }

            return listaRetornoEOL;
        }
    }
}
