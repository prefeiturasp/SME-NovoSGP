using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.Sondagem;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Sondagem.ObterConsolidacaoAlfabetizacaoCriticaEscrita
{
    public class ObterConsolidacaoAlfabetizacaoCriticaEscritaQueryHandler : IRequestHandler<ObterConsolidacaoAlfabetizacaoCriticaEscritaQuery, IEnumerable<SondagemConsolidacaoAlfabetizacaoCriticaEscritaDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterConsolidacaoAlfabetizacaoCriticaEscritaQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory;
            this.mediator = mediator;
        }
        public async Task<IEnumerable<SondagemConsolidacaoAlfabetizacaoCriticaEscritaDto>> 
            Handle(ObterConsolidacaoAlfabetizacaoCriticaEscritaQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicoSondagemConstants.Servico);
            var resposta = await httpClient.GetAsync(ServicoSondagemConstants.URL_CONSOLIDACAO_ALFABETIZACAO_CRITICA_ESCRITA);
            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<SondagemConsolidacaoAlfabetizacaoCriticaEscritaDto>>(json);
            }

            if (!resposta.IsSuccessStatusCode)
                await mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro ao obter Consolidação Alfabetização Crítica Escrita, código de erro: {resposta.StatusCode}",
                                                                  LogNivel.Critico,
                                                                  LogContexto.ApiSondagem,
                                                                  $"Mensagem: {await resposta.Content.ReadAsStringAsync()}, Request: {Newtonsoft.Json.JsonConvert.SerializeObject(resposta.RequestMessage)}"));
            return new List<SondagemConsolidacaoAlfabetizacaoCriticaEscritaDto>();
        }
    }
}
