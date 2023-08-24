using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQueryHandler : IRequestHandler<ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQuery, AbrangenciaCompactaVigenteRetornoEOLDTO>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<AbrangenciaCompactaVigenteRetornoEOLDTO> Handle(ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQuery request, CancellationToken cancellationToken)
        {
            var abrangencia = new AbrangenciaCompactaVigenteRetornoEOLDTO();

            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_ABRANGENCIA_COMPACTA_VIGENTE_PERFIL, request.Login, request.Perfil.ToString()));
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                abrangencia = JsonConvert.DeserializeObject<AbrangenciaCompactaVigenteRetornoEOLDTO>(json);
            }
            else
            {
                throw new Exception($"Não foi possível localizar abrangência para o login : {request.Login}.");
            }
            return abrangencia;
        }
    }
}
