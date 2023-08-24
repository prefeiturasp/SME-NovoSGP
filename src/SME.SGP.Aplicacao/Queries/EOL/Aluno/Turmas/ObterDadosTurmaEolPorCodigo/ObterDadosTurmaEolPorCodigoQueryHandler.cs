using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosTurmaEolPorCodigoQueryHandler : IRequestHandler<ObterDadosTurmaEolPorCodigoQuery,DadosTurmaEolDto>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterDadosTurmaEolPorCodigoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<DadosTurmaEolDto> Handle(ObterDadosTurmaEolPorCodigoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var url = string.Format(ServicosEolConstants.URL_TURMAS_DADOS, request.CodigoTurma);

            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException($"Não foram encontrados dados da turma {request.CodigoTurma} no EOL.");

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DadosTurmaEolDto>(json);
        }
    }
}