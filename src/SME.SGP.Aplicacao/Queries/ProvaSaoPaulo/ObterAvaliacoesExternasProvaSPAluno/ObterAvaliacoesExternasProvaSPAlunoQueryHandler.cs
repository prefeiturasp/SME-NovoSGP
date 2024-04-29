using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.ProvaSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterAvaliacoesExternasProvaSPAlunoQueryHandler : IRequestHandler<ObterAvaliacoesExternasProvaSPAlunoQuery, IEnumerable<AvaliacaoExternaProvaSPDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterAvaliacoesExternasProvaSPAlunoQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AvaliacaoExternaProvaSPDto>> Handle(ObterAvaliacoesExternasProvaSPAlunoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicoSerapConstants.ServicoSERApLegado);
            var resposta = await httpClient.GetAsync(string.Format(ServicoSerapConstants.URL_AVALIACOES_EXTERNAS_PROVA_SP_ALUNO_ANO_LETIVO, request.AnoLetivo, request.AlunoCodigo));
            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<AvaliacaoExternaProvaSPDto>>(json);
            }
            if (!resposta.IsSuccessStatusCode)
                await RegistraLogErro(resposta, $"Código Aluno: {request.AlunoCodigo}, Ano Letivo: {request.AnoLetivo}");

            return Enumerable.Empty<AvaliacaoExternaProvaSPDto>();
        }

        private async Task RegistraLogErro(HttpResponseMessage resposta, string parametros)
        {
            var mensagem = await resposta.Content.ReadAsStringAsync();
            var titulo = $"Ocorreu um erro ao obter Avaliações Externas Prova SP - SERAp, código de erro: {resposta.StatusCode}";
            await mediator.Send(new SalvarLogViaRabbitCommand(titulo,
                                                              LogNivel.Critico,
                                                              LogContexto.ApiSERAp,
                                                              $"Mensagem: {mensagem}, Parametros:{parametros}, Request: {JsonConvert.SerializeObject(resposta.RequestMessage)}"));
        }
    }

    public static class ListExtension
    {
        public static string SerializarJsonTipoQuestaoAvaliacoesExternasProvaSP(this List<AvaliacaoExternaProvaSPDto> source)
        => JsonConvert.SerializeObject(source);
    }
}
