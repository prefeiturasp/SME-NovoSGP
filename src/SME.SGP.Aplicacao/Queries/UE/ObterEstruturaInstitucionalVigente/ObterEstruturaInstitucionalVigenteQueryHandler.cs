using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEstruturaInstitucionalVigenteQueryHandler : IRequestHandler<ObterEstruturaInstitucionalVigenteQuery, EstruturaInstitucionalRetornoEolDTO>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;
        private const string BaseUrl = ServicosEolConstants.URL_ABRANGENCIA_ESTRUTRURA_VIGENTE;

        public ObterEstruturaInstitucionalVigenteQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<EstruturaInstitucionalRetornoEolDTO> Handle(ObterEstruturaInstitucionalVigenteQuery request, CancellationToken cancellationToken)
        {
            EstruturaInstitucionalRetornoEolDTO resultado = null;

            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var url = new StringBuilder(BaseUrl);

            resultado = new EstruturaInstitucionalRetornoEolDTO();

            var resposta = await httpClient.GetAsync($"{url}/{request.CodigoDre}", cancellationToken);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                var parcial = JsonConvert.DeserializeObject<EstruturaInstitucionalRetornoEolDTO>(json);

                if (parcial != null)
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