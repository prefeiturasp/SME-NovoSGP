using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPapPorAnoLetivoQueryHandler : IRequestHandler<ObterTurmasPapPorAnoLetivoQuery,IEnumerable<TurmasPapDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;


        public ObterTurmasPapPorAnoLetivoQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<TurmasPapDto>> Handle(ObterTurmasPapPorAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var url = string.Format(ServicosEolConstants.URL_TURMAS_TURMAS_PAP,request.AnoLetivo);

            try
            {
                var resposta = await httpClient.GetAsync(url, cancellationToken);
                if (!resposta.IsSuccessStatusCode || resposta.StatusCode == HttpStatusCode.NoContent)
                    return Enumerable.Empty<TurmasPapDto>();
                var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
                return  JsonConvert.DeserializeObject<List<TurmasPapDto>>(json);
            }
            catch (Exception e)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao obter as turmas pap no EOL - AnoLetivo:{string.Join(",", request.AnoLetivo)}", LogNivel.Negocio, LogContexto.ApiEol, e.Message), cancellationToken);
                throw;
            }
        }
    }
}