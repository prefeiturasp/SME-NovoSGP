﻿using MediatR;
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
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var url = string.Format(ServicosEolConstants.URL_ALUNOS_TURMAS_ANO_LETIVO_MATRICULA_TURMA_TIPO_TURMA, request.CodigoAluno, request.AnoLetivo, request.FiltrarSituacaoMatricula, request.TipoTurma);
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
                    var httpContentResult = (await resposta?.Content?.ReadAsStringAsync())?.ToString();
                    var respostaErro = (resposta?.Content).NaoEhNulo() ? httpContentResult : erro;
                    throw new NegocioException(respostaErro);
                }
            }
            catch (Exception e)
            {

                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao obter os dados do aluno no EOL - Código:{request.CodigoAluno}, Ano:{request.AnoLetivo}, FiltrarSituacaoMatricula:{request.FiltrarSituacaoMatricula} - Erro:{e.Message}", LogNivel.Negocio, LogContexto.Turma, e.Message));
                throw e;
            }
        }
    }
}
