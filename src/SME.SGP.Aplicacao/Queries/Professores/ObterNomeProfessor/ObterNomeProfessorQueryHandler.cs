using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterNomeProfessorQueryHandler : IRequestHandler<ObterNomeProfessorQuery, string>
    {
        private readonly IHttpClientFactory httpClientFactory;
        
        public ObterNomeProfessorQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<string> Handle(ObterNomeProfessorQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_PROFESSORES,request.RFProfessor));

            if (!resposta.IsSuccessStatusCode)
                return default;

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                return default;

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<string>(json); 
        }
    }
}
