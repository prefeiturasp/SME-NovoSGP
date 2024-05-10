using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
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
    public class ObterComponentesCurricularesEOLPorTurmaECodigoUeQueryHandler : IRequestHandler<ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery, IEnumerable<ComponenteCurricularDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterComponentesCurricularesEOLPorTurmaECodigoUeQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ComponenteCurricularDto>> Handle(ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var queryParamTurmas = string.Empty;

            if (request.CodigosDeTurmas.NaoEhNulo() && request.CodigosDeTurmas.Any())
            {
                var codigosTurmas = String.Join("&turmas=", request.CodigosDeTurmas);
                queryParamTurmas = $"?turmas={codigosTurmas}";
            }

            var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_COMPONENTES_CURRICULARES_UES_TURMAS, request.CodigoUe) + $"{queryParamTurmas}");

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                var componenteCurricularEol = JsonConvert.DeserializeObject<List<ComponenteCurricularEol>>(json);

                var componentesCurricularesSgp = await mediator.Send(new ObterInfoPedagogicasComponentesCurricularesPorIdsQuery(componenteCurricularEol.ObterCodigos()));
                componenteCurricularEol.PreencherInformacoesPegagogicasSgp(componentesCurricularesSgp);
                return componenteCurricularEol.Select(cc => new ComponenteCurricularDto()
                {
                    Codigo = cc.Codigo.ToString(),
                    Descricao = cc.Descricao,
                    LancaNota = cc.LancaNota,
                    Regencia = cc.Regencia,
                    TerritorioSaber = cc.TerritorioSaber
                });
            }
            else throw new NegocioException("Não foi possível obter Componentes Curriculares.");

        }
    }
}
