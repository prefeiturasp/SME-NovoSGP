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
    public class ObterTurmasAlunoPorFiltroQueryHandler : IRequestHandler<ObterTurmasAlunoPorFiltroQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterTurmasAlunoPorFiltroQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterTurmasAlunoPorFiltroQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var url = $"alunos/{request.CodidoAluno}/turmas/anosLetivos/{request.AnoLetivo}/matriculaTurma/{request.FiltrarSituacaoMatricula}";
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
                    string erro = $"Não foi possível obter os dados do aluno no EOL - HttpCode {(int)resposta.StatusCode} - erro: {JsonConvert.SerializeObject(resposta.RequestMessage)}";

                    await mediator.Send(new SalvarLogViaRabbitCommand(erro, LogNivel.Negocio, LogContexto.Turma, string.Empty));
                    var respostaErro = resposta?.Content != null ? resposta?.Content?.ReadAsStringAsync()?.Result.ToString() : erro;
                    throw new NegocioException(respostaErro);
                }
            }
            catch (Exception e)
            {

                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao obter os dados do aluno no EOL - Código:{request.CodidoAluno}, Ano:{request.AnoLetivo}, FiltrarSituacaoMatricula:{request.FiltrarSituacaoMatricula} - Erro:{e.Message}", LogNivel.Negocio, LogContexto.Turma, e.Message));
                throw e;
            }
        }
    }
}
