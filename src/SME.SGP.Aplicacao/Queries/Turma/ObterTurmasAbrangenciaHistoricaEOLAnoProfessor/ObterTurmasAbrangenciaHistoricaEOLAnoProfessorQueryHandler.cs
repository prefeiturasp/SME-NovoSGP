using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasAbrangenciaHistoricaEOLAnoProfessorQueryHandler : IRequestHandler<ObterTurmasAbrangenciaHistoricaEOLAnoProfessorQuery, List<AbrangenciaTurmaRetornoEolDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterTurmasAbrangenciaHistoricaEOLAnoProfessorQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<List<AbrangenciaTurmaRetornoEolDto>> Handle(ObterTurmasAbrangenciaHistoricaEOLAnoProfessorQuery request, CancellationToken cancellationToken)
        {
            int anoLetivo = request.AnoLetivo;
            string professorRf = request.ProfessorRf;

            var httpClient = httpClientFactory.CreateClient("servicoEOL");

            var resposta = await httpClient.GetAsync($"turmas/anos-letivos/{anoLetivo}/professor/{professorRf}/turmas-historicas-geral");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<AbrangenciaTurmaRetornoEolDto>>(json);
            }
            else
            {
                string erro = $"Não foi possível obter as turmas históricas do professor no EOL - HttpCode {(int)resposta.StatusCode} - AnoLetivo({request.AnoLetivo}), RF({request.ProfessorRf}). - erro: {JsonConvert.SerializeObject(resposta.RequestMessage)}";
                await mediator.Send(new SalvarLogViaRabbitCommand(erro, LogNivel.Negocio, LogContexto.Turma, string.Empty));
                throw new NegocioException(erro);
            }
        }
    }
}
