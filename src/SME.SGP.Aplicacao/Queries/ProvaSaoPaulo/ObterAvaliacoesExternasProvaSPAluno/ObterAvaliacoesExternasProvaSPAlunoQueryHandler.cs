using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.ProvaSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterAvaliacoesExternasProvaSPAlunoQueryHandler : IRequestHandler<ObterAvaliacoesExternasProvaSPAlunoQuery, IEnumerable<AvaliacaoExternaProvaSPDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterAvaliacoesExternasProvaSPAlunoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<AvaliacaoExternaProvaSPDto>> Handle(ObterAvaliacoesExternasProvaSPAlunoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicoSerapConstants.ServicoSERApLegado);
            var resposta = await httpClient.GetAsync(string.Format(ServicoSerapConstants.URL_AVALIACOES_EXTERNAS_PROVA_SP_ALUNO_ANO_LETIVO, request.AlunoCodigo, request.AnoLetivo));
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<AvaliacaoExternaProvaSPDto>>(json);
            }
            return Enumerable.Empty<AvaliacaoExternaProvaSPDto>();
        }
    }

    public static class ListExtension
    {
        public static string SerializarJsonTipoQuestaoAvaliacoesExternasProvaSP(this List<AvaliacaoExternaProvaSPDto> source)
        => JsonConvert.SerializeObject(source);
    }
}
