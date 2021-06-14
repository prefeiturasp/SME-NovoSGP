using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasDoProfessorQueryHandler : IRequestHandler<ObterTurmasDoProfessorQuery, IEnumerable<ProfessorTurmaReposta>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterTurmasDoProfessorQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<ProfessorTurmaReposta>> Handle(ObterTurmasDoProfessorQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");

            var resposta = await httpClient.GetAsync($"/api/professores/{request.ProfessorRf}/turmas");

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<ProfessorTurmaReposta>>(json);
            }

            return Enumerable.Empty<ProfessorTurmaReposta>();
        }
    }
}
