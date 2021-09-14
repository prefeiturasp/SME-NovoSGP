using MediatR;
using Newtonsoft.Json;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ProfessoresTurmaDisciplinaQueryHandler : IRequestHandler<ProfessoresTurmaDisciplinaQuery, List<ProfessorAtribuidoTurmaDisciplinaDTO>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ProfessoresTurmaDisciplinaQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<List<ProfessorAtribuidoTurmaDisciplinaDTO>> Handle(ProfessoresTurmaDisciplinaQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var datasParaEnvio = string.Join("&dataTicks=", request.Data.Ticks);
            var resposta = await httpClient.GetAsync($"professores/{request.CodigoTurma}/disciplinas/{request.DisciplinaId}/atribuicao/data?dataTicks={datasParaEnvio}");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ProfessorAtribuidoTurmaDisciplinaDTO>>(json);
            }
            else
            {
                string erro = $"Não foi possível consultar as atribuições da turma no EOL - HttpCode {(int)resposta.StatusCode} - Turma:{request.CodigoTurma} Disciplina:{request.DisciplinaId} Data:{request.Data.ToShortDateString()}";

                SentrySdk.AddBreadcrumb(erro);
                throw new NegocioException(erro);
            }
        }
    }
}
