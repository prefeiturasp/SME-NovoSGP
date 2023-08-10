using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasAlunoPorFiltroPlanoAeeQueryHandler : IRequestHandler<ObterTurmasAlunoPorFiltroPlanoAeeQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        public ObterTurmasAlunoPorFiltroPlanoAeeQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterTurmasAlunoPorFiltroPlanoAeeQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var url = $"alunos/{request.CodigoAluno}/turmas/anosLetivos/{request.AnoLetivo}/matriculaTurma/{request.FiltrarSituacaoMatricula}/tipoTurma/{request.TipoTurma}";

            try
            {
                var resposta = await httpClient.GetAsync(url);
                if (resposta.IsSuccessStatusCode)
                {
                    var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
                    return JsonConvert.DeserializeObject<List<AlunoPorTurmaResposta>>(json);
                }

                var erro = $"Não foi possível obter os dados do aluno no EOL - HttpCode {(int) resposta.StatusCode} - erro: {JsonConvert.SerializeObject(resposta.RequestMessage)}";
                var respostaErro = resposta?.Content?.ReadAsStringAsync(cancellationToken)?.Result.ToString();

                await mediator.Send(new SalvarLogViaRabbitCommand(erro, LogNivel.Negocio, LogContexto.Turma, respostaErro), cancellationToken);
                return Enumerable.Empty<AlunoPorTurmaResposta>();
            }
            catch (Exception e)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao obter os dados do aluno no EOL - Código:{request.CodigoAluno}, Ano:{request.AnoLetivo}, FiltrarSituacaoMatricula:{request.FiltrarSituacaoMatricula} - Erro:{e.Message}", LogNivel.Negocio, LogContexto.Turma, e.Message));
                throw;
            }
        }
    }
}