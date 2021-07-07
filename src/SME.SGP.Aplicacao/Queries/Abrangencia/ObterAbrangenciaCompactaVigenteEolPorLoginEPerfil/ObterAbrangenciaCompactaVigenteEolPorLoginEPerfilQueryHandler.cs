using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dto;
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

            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"abrangencia/compacta-vigente/{request.Login}/perfil/{request.Perfil.ToString()}");
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
