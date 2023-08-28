using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessorTitularPorTurmaEComponenteCurricularQueryHandler : IRequestHandler<ObterProfessorTitularPorTurmaEComponenteCurricularQuery, ProfessorTitularDisciplinaEol>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;
        public ObterProfessorTitularPorTurmaEComponenteCurricularQueryHandler(IHttpClientFactory httpClientFactory,IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ProfessorTitularDisciplinaEol> Handle(ObterProfessorTitularPorTurmaEComponenteCurricularQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dadosProfessor = new ProfessorTitularDisciplinaEol();

                var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
                var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_PROFESSORES_TITULAR_TURMAS_COMPONENTES, request.TurmaCodigo, request.ComponenteCurricularCodigo));
                if (resposta.IsSuccessStatusCode)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    dadosProfessor = JsonConvert.DeserializeObject<ProfessorTitularDisciplinaEol>(json);
                }
                return dadosProfessor;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao Obter Professor Titular da Turma {request.TurmaCodigo} para o Componente Curricular {request.ComponenteCurricularCodigo}", LogNivel.Critico, LogContexto.Aula, ex.Message,innerException: ex.InnerException.ToString(),rastreamento:ex.StackTrace), cancellationToken);
                throw;
            }
        }
    }
}
