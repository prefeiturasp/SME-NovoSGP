using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Dto;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaPorLoginPerfilQueryHandler : IRequestHandler<ObterAbrangenciaPorLoginPerfilQuery, AbrangenciaRetornoEolDto>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterAbrangenciaPorLoginPerfilQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<AbrangenciaRetornoEolDto> Handle(ObterAbrangenciaPorLoginPerfilQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            
            var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_FUNCIONARIOS_PERFIS_TURMAS,request.Login, request.Perfil.ToString()));

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AbrangenciaRetornoEolDto>(json);
            }
            return default;
        }
    }
}
