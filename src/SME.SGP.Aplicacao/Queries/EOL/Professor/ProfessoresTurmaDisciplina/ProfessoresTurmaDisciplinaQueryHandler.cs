using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ProfessoresTurmaDisciplinaQueryHandler : IRequestHandler<ProfessoresTurmaDisciplinaQuery, List<ProfessorAtribuidoTurmaDisciplinaDTO>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ProfessoresTurmaDisciplinaQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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
                string erro = $"Não foi possível consultar as atribuições da turma no EOL - HttpCode {(int)resposta.StatusCode} - Turma:{request.CodigoTurma} Disciplina:{request.DisciplinaId} Data:{request.Data.ToShortDateString()} - erro : {resposta.Content.ReadAsStringAsync()}";
                await mediator.Send(new SalvarLogViaRabbitCommand(erro, LogNivel.Negocio, LogContexto.Turma, string.Empty));
                throw new NegocioException(erro);
            }
        }
    }
}
