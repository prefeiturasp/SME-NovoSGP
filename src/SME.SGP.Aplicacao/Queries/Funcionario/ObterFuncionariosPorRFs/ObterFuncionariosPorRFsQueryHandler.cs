using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Funcionario.ObterListaNomePorListaRF
{
    public class ObterFuncionariosPorRFsQueryHandler : IRequestHandler<ObterFuncionariosPorRFsQuery, IEnumerable<ProfessorResumoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterFuncionariosPorRFsQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<ProfessorResumoDto>> Handle(ObterFuncionariosPorRFsQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");

            var resposta = await httpClient.PostAsync($"funcionarios/BuscarPorListaRF", new StringContent(JsonConvert.SerializeObject(request.CodigosRf),
                Encoding.UTF8, "application/json-patch+json"), cancellationToken);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
                return JsonConvert.DeserializeObject<IEnumerable<ProfessorResumoDto>>(json);
            }

            throw new Exception($"Não foi possível localizar os rfs : {string.Join(",",request.CodigosRf)}.");
        } 
    }
}