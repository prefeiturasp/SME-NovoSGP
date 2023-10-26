using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresTitularesPorUeQueryHandler : IRequestHandler<ObterProfessoresTitularesPorUeQuery, IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterProfessoresTitularesPorUeQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> Handle(ObterProfessoresTitularesPorUeQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var url = string.Format(ServicosEolConstants.URL_PROFESSORES_TITULARES_UE, request.UeCodigo, request.DataReferencia.ToString("yyyy-MM-dd"));
            
            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode)
                return default;

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                return default;

            var json = await resposta.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<IEnumerable<ProfessorTitularDisciplinaEol>>(json);
        }
    }
}