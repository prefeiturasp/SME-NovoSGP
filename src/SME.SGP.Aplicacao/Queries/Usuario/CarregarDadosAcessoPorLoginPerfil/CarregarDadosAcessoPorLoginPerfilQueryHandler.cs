using System;
using System.Net.Http;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    class CarregarDadosAcessoPorLoginPerfilQueryHandler : IRequestHandler<CarregarDadosAcessoPorLoginPerfilQuery, RetornoDadosAcessoUsuarioSgpDto>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public CarregarDadosAcessoPorLoginPerfilQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<RetornoDadosAcessoUsuarioSgpDto> Handle(CarregarDadosAcessoPorLoginPerfilQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            
            var url = string.Format(ServicosEolConstants.URL_AUTENTICACAO_SGP_CARREGAR_DADOS_ACESSO_USUARIOS_PERFIS, request.Login, request.PerfilGuid);

            if (request.AdministradorSuporte.NaoEhNulo())
                url += $"?loginAdm={request.AdministradorSuporte.Login}&nomeAdm={request.AdministradorSuporte.Nome}";
            
            var resposta = await httpClient.GetAsync(url);                
            
            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Não foi possivel obter os dados de acesso");

            var json = await resposta.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<RetornoDadosAcessoUsuarioSgpDto>(json);
        }
    }
}