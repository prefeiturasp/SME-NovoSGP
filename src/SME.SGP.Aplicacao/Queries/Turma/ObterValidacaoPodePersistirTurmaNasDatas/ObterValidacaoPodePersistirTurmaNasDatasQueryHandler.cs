using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterValidacaoPodePersistirTurmaNasDatasQueryHandler : IRequestHandler<ObterValidacaoPodePersistirTurmaNasDatasQuery, List<PodePersistirNaDataRetornoEolDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterValidacaoPodePersistirTurmaNasDatasQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<List<PodePersistirNaDataRetornoEolDto>> Handle(ObterValidacaoPodePersistirTurmaNasDatasQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var datasParaEnvio = string.Join("&dataTicks=", request.DateTimes.Select(a => a.Ticks).ToArray());
            var resposta = await httpClient.GetAsync($"professores/{request.CodigoRf}/turmas/{request.TurmaCodigo}/disciplinas/{request.ComponenteCurricularCodigo}/atribuicao/recorrencia/verificar/datas?dataTicks={datasParaEnvio}");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<PodePersistirNaDataRetornoEolDto>>(json);
            }
            else
            {
                string erro = $"Não foi possível validar datas para a atribuição do professor no EOL - HttpCode {(int)resposta.StatusCode} - {string.Join("-", request.DateTimes)} - erro: {JsonConvert.SerializeObject(resposta.RequestMessage)}";
                await mediator.Send(new SalvarLogViaRabbitCommand(erro, LogNivel.Negocio, LogContexto.Turma, string.Empty));

                throw new NegocioException(erro);
            }
        }
    }
}