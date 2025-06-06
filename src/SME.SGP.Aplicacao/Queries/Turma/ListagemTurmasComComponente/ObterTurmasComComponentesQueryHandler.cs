﻿using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComComponentesQueryHandler : IRequestHandler<ObterTurmasComComponentesQuery, PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;    

        public ObterTurmasComComponentesQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>> Handle(ObterTurmasComComponentesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var turmas = new PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>();

                var turmaCodigo = request.TurmaCodigo.EhNulo() ? 0 : long.Parse(request.TurmaCodigo);

                var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
                var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_TURMAS_UES_MODALIDADE_ANOS_COMPONENTES, request.UeCodigo, (int)request.Modalidade, request.AnoLetivo) + $"?codigoTurma={turmaCodigo}&ehProfessor={request.EhProfessor}&codigoRf={request.CodigoRf}&qtdeRegistros={request.QtdeRegistros}&qtdeRegistrosIgnorados={request.QtdeRegistrosIgnorados}&consideraHistorico={request.ConsideraHistorico}&periodoEscolarInicio={request.PeriodoEscolarInicio.Ticks}&anosInfantilDesconsiderar={request.AnosInfantilDesconsiderar}");

                if (!resposta.IsSuccessStatusCode)
                    return turmas;

                var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
                turmas = JsonConvert.DeserializeObject<PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>>(json);

                return turmas;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro ao obter as turmas com componentes da consolidação do conselho de classe", LogNivel.Critico, LogContexto.ConselhoClasse, ex.Message, "SGP", ex.StackTrace, ex.InnerException?.ToString()));
                throw;
            }
        }
    }
}
