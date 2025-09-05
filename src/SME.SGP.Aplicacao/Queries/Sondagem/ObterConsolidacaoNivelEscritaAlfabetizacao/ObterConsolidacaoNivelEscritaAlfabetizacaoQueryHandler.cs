using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.Sondagem;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Sondagem.ObterConsolidacaoNivelEscritaAlfabetizacao
{
    public class ObterConsolidacaoNivelEscritaAlfabetizacaoQueryHandler : IRequestHandler<ObterConsolidacaoNivelEscritaAlfabetizacaoQuery, IEnumerable<SondagemConsolidacaoNivelEscritaAlfabetizacaoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterConsolidacaoNivelEscritaAlfabetizacaoQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory;
            this.mediator = mediator;
        }
        public async Task<IEnumerable<SondagemConsolidacaoNivelEscritaAlfabetizacaoDto>> 
            Handle(ObterConsolidacaoNivelEscritaAlfabetizacaoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicoSondagemConstants.Servico);
            var resposta = await httpClient.GetAsync(ServicoSondagemConstants.URL_CONSOLIDACAO_NIVEL_ESCRITA_ALFABETIZACAO);
            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<SondagemConsolidacaoNivelEscritaAlfabetizacaoDto>>(json);
            }
            if (!resposta.IsSuccessStatusCode)
                await mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro ao obter Consolidação Nível Escrita Alfabetização, código de erro: {resposta.StatusCode}",
                                                                  LogNivel.Critico,
                                                                  LogContexto.ApiSondagem,
                                                                  $"Mensagem: {await resposta.Content.ReadAsStringAsync()}, Request: {JsonConvert.SerializeObject(resposta.RequestMessage)}"));
            return new List<SondagemConsolidacaoNivelEscritaAlfabetizacaoDto>();
        }
    }
}
