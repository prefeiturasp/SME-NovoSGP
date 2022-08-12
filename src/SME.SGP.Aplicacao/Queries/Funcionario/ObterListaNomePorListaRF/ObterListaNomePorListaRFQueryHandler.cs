using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Funcionario.ObterListaNomePorListaRF
{
    public class ObterListaNomePorListaRFQueryHandler : IRequestHandler<ObterListaNomePorListaRFQuery, IEnumerable<ProfessorResumoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterListaNomePorListaRFQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<ProfessorResumoDto>> Handle(ObterListaNomePorListaRFQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");

            var resposta = await httpClient.PostAsync($"funcionarios/BuscarPorListaRF",
                new StringContent(JsonConvert.SerializeObject(request.CodigosRf),
                Encoding.UTF8, "application/json-patch+json"));

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<ProfessorResumoDto>>(json);
            }
            else
            {
                throw new Exception($"Não foi possível localizar os rfs : {string.Join(",",request.CodigosRf)}.");
            }
        } 
    }
}