using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDisciplinasPorCodigoTurmaQueryHandler : IRequestHandler<ObterDisciplinasPorCodigoTurmaQuery, IEnumerable<DisciplinaResposta>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        public ObterDisciplinasPorCodigoTurmaQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<DisciplinaResposta>> Handle(ObterDisciplinasPorCodigoTurmaQuery request, CancellationToken cancellationToken)
        {
            var url = $"funcionarios/turmas/{request.CodigoTurma}/disciplinas";
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync(url);

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<DisciplinaResposta>>(json);
            }

            if (resposta.StatusCode == HttpStatusCode.BadRequest)
                throw new NegocioException("Ocorreu um erro na tentativa de buscar as disciplinas no EOL.");

            return null;
        }
    }
}
