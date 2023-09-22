using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEstruturaInstitucionalVigentePorDreQueryHandler : IRequestHandler<ObterEstruturaInstitucionalVigentePorDreQuery, EstruturaInstitucionalRetornoEolDTO>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterEstruturaInstitucionalVigentePorDreQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<EstruturaInstitucionalRetornoEolDTO> Handle(ObterEstruturaInstitucionalVigentePorDreQuery request, CancellationToken cancellationToken)
        {
            EstruturaInstitucionalRetornoEolDTO resultado;

            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            resultado = new EstruturaInstitucionalRetornoEolDTO();

            var resposta = await httpClient.GetAsync($"{ServicosEolConstants.URL_ABRANGENCIA_ESTRUTURA_VIGENTE}/{request.CodigoDre}", cancellationToken);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                var parcial = JsonConvert.DeserializeObject<EstruturaInstitucionalRetornoEolDTO>(json);

                if (parcial.NaoEhNulo())
                    resultado.Dres.AddRange(parcial.Dres);
            }
            else
            {
                var httpContentResult = await resposta.Content?.ReadAsStringAsync();
                var erro = $"Ocorreu um erro na tentativa de buscar os dados de Estrutura Institucional Vigente por Dre: {request.CodigoDre} - HttpCode {resposta.StatusCode} - Body {httpContentResult ?? string.Empty} - erro: {JsonConvert.SerializeObject(resposta.RequestMessage)}";
                await mediator.Send(new SalvarLogViaRabbitCommand(erro, LogNivel.Negocio, LogContexto.Turma, string.Empty));
            }

            return resultado;
        }
    }
}