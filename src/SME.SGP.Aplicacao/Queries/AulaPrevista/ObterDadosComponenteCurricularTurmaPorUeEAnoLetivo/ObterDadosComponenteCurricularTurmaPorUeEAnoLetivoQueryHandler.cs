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
            var listaRetorno = new List<DadosTurmaAulasAutomaticaDto>();

            using (var httpClient = httpClientFactory.CreateClient("servicoEOL"))
            {
                var componentesCurriculares = String.Join("&componentesCurriculares=", request.ComponentesCurriculares);
                var resposta = await httpClient.GetAsync($"/api/v1/componentes-curriculares/dados-aula-turma?anoLetivo={request.AnoLetivo}&ueCodigo={request.UeCodigo}&componentesCurriculares={componentesCurriculares}");

                if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    var listaRetornoEOL = JsonConvert.DeserializeObject<IEnumerable<DadosTurmaAulasAutomaticaDto>>(json) as List<DadosTurmaAulasAutomaticaDto>;
                    if (listaRetornoEOL.Any())
                        listaRetorno.AddRange(listaRetornoEOL);
                }

            }

            return listaRetorno;
        }
    }
}
