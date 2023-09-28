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
    public class ObterEstruturaInstuticionalVigentePorTurmaQueryHandler : IRequestHandler<ObterEstruturaInstuticionalVigentePorTurmaQuery, EstruturaInstitucionalRetornoEolDTO>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterEstruturaInstuticionalVigentePorTurmaQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<EstruturaInstitucionalRetornoEolDTO> Handle(ObterEstruturaInstuticionalVigentePorTurmaQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var filtroTurmas = new StringContent(JsonConvert.SerializeObject(request.CodigosTurma ?? new string[] { }), UnicodeEncoding.UTF8, "application/json");

            var resposta = await httpClient.PostAsync(ServicosEolConstants.URL_ABRANGENCIA_ESTRUTURA_VIGENTE, filtroTurmas);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EstruturaInstitucionalRetornoEolDTO>(json);
            }

            throw new NegocioException($"Ocorreu um erro na tentativa de buscar os dados de Estrutura Institucional Vigente");
        }
    }
}