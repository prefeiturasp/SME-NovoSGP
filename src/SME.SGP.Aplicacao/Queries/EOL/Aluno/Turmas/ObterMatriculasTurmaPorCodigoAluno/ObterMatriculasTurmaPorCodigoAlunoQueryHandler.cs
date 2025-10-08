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
    public class ObterMatriculasTurmaPorCodigoAlunoQueryHandler : IRequestHandler<ObterMatriculasTurmaPorCodigoAlunoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterMatriculasTurmaPorCodigoAlunoQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterMatriculasTurmaPorCodigoAlunoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var operadorAnoLetivo = request.AnoLetivo.HasValue ? $"&" : string.Empty;
            var queryParam = $"{(request.AnoLetivo.HasValue ? $"AnoLetivo={request.AnoLetivo.Value}{operadorAnoLetivo}" : string.Empty)}";
            queryParam = queryParam + $"{(request.DataAula.HasValue ? $"dataAulaTicks={request.DataAula.Value.Ticks}" : string.Empty)}";
            var url = string.Format(ServicosEolConstants.URL_TURMAS_ALUNOS, request.CodigoAluno) + $"{(!string.IsNullOrEmpty(queryParam) ? $"?{queryParam}" : string.Empty)}";
            try
            {
                var resposta = await httpClient.GetAsync(url);
                if (resposta.IsSuccessStatusCode)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<AlunoPorTurmaResposta>>(json);
                }
                else
                {
                    string respostaHttp = $"HttpCode {(int)resposta.StatusCode} - HttpMessage: {JsonConvert.SerializeObject(resposta.RequestMessage)}";
                    var httpContentResult = (await resposta?.Content?.ReadAsStringAsync())?.ToString();
                    var respostaErro = (resposta?.Content).NaoEhNulo() ? httpContentResult : respostaHttp;
                    throw new ErroInternoException(respostaErro);
                }
            }
            catch (Exception e)
            {

                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao obter as matrículas/turma do aluno no EOL - Aluno: {request.CodigoAluno} - Erro: {e.Message}", 
                                                                    LogNivel.Critico, 
                                                                    LogContexto.Turma, e.Message));
                throw;
            }
        }
    }
}
