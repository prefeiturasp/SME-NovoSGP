using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosAtivosPorUeENomeQueryHandler : IRequestHandler<ObterAlunosAtivosPorUeENomeQuery, IEnumerable<AlunoParaAutoCompleteAtivoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterAlunosAtivosPorUeENomeQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<AlunoParaAutoCompleteAtivoDto>> Handle(ObterAlunosAtivosPorUeENomeQuery request, CancellationToken cancellationToken)
        {
            var alunos = Enumerable.Empty<AlunoParaAutoCompleteAtivoDto>();

            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_ALUNOS_UES_AUTOCOMPLETE_ATIVOS, request.UeCodigo) + $"?DataReferencia={request.DataReferencia.ToString("s")}&alunoNome={request.AlunoNome}&alunoCodigo={request.AlunoCodigo}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                alunos = JsonConvert.DeserializeObject<List<AlunoParaAutoCompleteAtivoDto>>(json);
            }
            return alunos;
        }
    }
}
