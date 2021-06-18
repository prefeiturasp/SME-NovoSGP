using MediatR;
using Newtonsoft.Json;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasAbrangenciaHistoricaEOLAnoProfessorQueryHandler : IRequestHandler<ObterTurmasAbrangenciaHistoricaEOLAnoProfessorQuery, List<AbrangenciaTurmaRetornoEolDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        public ObterTurmasAbrangenciaHistoricaEOLAnoProfessorQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<List<AbrangenciaTurmaRetornoEolDto>> Handle(ObterTurmasAbrangenciaHistoricaEOLAnoProfessorQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");            
            var resposta = await httpClient.GetAsync($"turmas/anos-letivos/{request.AnoLetivo}/professor/{request.ProfessorRf}/turmas-historicas-geral");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<AbrangenciaTurmaRetornoEolDto>>(json);
            }
            else
            {
                string erro = $"Não foi possível obter as turmas históricas do professor no EOL - HttpCode {(int)resposta.StatusCode} - AnoLetivo({request.AnoLetivo}), RF({request.ProfessorRf}).";
                SentrySdk.AddBreadcrumb(erro);
                throw new NegocioException(erro);
            }
        }
    }
}
