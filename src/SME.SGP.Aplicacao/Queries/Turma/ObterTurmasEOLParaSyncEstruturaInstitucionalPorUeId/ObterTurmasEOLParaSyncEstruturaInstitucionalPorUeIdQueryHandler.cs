using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasEOLParaSyncEstruturaInstitucionalPorUeIdQueryHandler : IRequestHandler<ObterTurmasEOLParaSyncEstruturaInstitucionalPorUeIdQuery, IEnumerable<Turma>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterTurmasEOLParaSyncEstruturaInstitucionalPorUeIdQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<Turma>> Handle(ObterTurmasEOLParaSyncEstruturaInstitucionalPorUeIdQuery request, CancellationToken cancellationToken)
        {
            var turmas = new List<Turma>();

            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"alunos/ues/{request.UeId}/autocomplete/ativos?DataReferencia"); // TODO : Falta ajustar com o endpoint real de turmas do eol
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                turmas = JsonConvert.DeserializeObject<List<Turma>>(json);
            }
            return turmas;
        }
    }
}
