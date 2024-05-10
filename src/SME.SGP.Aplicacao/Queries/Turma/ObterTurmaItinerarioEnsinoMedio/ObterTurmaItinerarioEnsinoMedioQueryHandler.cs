using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaItinerarioEnsinoMedioQueryHandler : IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>
        {
            private readonly IHttpClientFactory httpClientFactory;

            public ObterTurmaItinerarioEnsinoMedioQueryHandler(IHttpClientFactory httpClientFactory)
            {
                this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            }

            public async Task<IEnumerable<TurmaItinerarioEnsinoMedioDto>> Handle(ObterTurmaItinerarioEnsinoMedioQuery request, CancellationToken cancellationToken)
            {
                var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
                var resposta = await httpClient.GetAsync(ServicosEolConstants.URL_TURMAS_ITINERARIO_ENSINO_MEDIO);
                if (resposta.IsSuccessStatusCode)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<TurmaItinerarioEnsinoMedioDto>>(json);
                }
                return default;
            }
        }
    }
