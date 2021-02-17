using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
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
            var alunos = new List<AlunoParaAutoCompleteAtivoDto>();

            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"alunos/ues/{request.UeCodigo}/autocomplete/ativos?DataReferencia={request.DataReferencia.ToString("s")}&alunoNome={request.AlunoNome}&alunoCodigo={request.AlunoCodigo}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                alunos = JsonConvert.DeserializeObject<List<AlunoParaAutoCompleteAtivoDto>>(json);
            }
            return alunos;
        }
    }
}
