using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dto;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaParaSupervisorQueryHandler : IRequestHandler<ObterAbrangenciaParaSupervisorQuery, AbrangenciaRetornoEolDto>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterAbrangenciaParaSupervisorQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<AbrangenciaRetornoEolDto> Handle(ObterAbrangenciaParaSupervisorQuery request, CancellationToken cancellationToken)
        {
           var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            
           var parametros = new StringContent(JsonConvert.SerializeObject(request.UesIds), Encoding.UTF8, "application/json");
            
           var resposta = await httpClient.PostAsync(ServicosEolConstants.URL_FUNCIONARIOS_TURMAS,parametros);

           if (resposta.IsSuccessStatusCode)
           {
                var jsonRetorno = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AbrangenciaRetornoEolDto>(jsonRetorno);
           }
           
           throw new NegocioException(MensagemNegocioEOL.HOUVE_ERRO_AO_TENTAR_OBTER_ABRANGENCIA_DO_EOL);
        }
    }
}
