using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosResponsavelQueryHandler : IRequestHandler<ObterDadosResponsavelQuery, IEnumerable<DadosResponsavelAlunoEolDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterDadosResponsavelQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<DadosResponsavelAlunoEolDto>> Handle(ObterDadosResponsavelQuery request, CancellationToken cancellationToken)
        {
            var dadosReponsaveis = Enumerable.Empty<DadosResponsavelAlunoEolDto>();

            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var url = string.Format(ServicosEolConstants.URL_ALUNOS_RESPONSAVEIS, request.CpfResponsavel);

            var resposta = await httpClient.GetAsync(url);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                dadosReponsaveis = JsonConvert.DeserializeObject<List<DadosResponsavelAlunoEolDto>>(json);
            }

            return dadosReponsaveis;
        }
    }
}
