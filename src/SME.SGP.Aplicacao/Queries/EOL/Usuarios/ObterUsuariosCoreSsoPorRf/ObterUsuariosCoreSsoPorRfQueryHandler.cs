using MediatR;
using SME.SGP.Infra;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.EOL.Usuarios.ObterUsuariosCoreSsoPorRf
{
    public class ObterUsuariosCoreSsoPorRfQueryHandler : IRequestHandler<ObterUsuariosCoreSsoPorRfQuery, UsuarioCoreSsoDto>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ObterUsuariosCoreSsoPorRfQueryHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<UsuarioCoreSsoDto> Handle(ObterUsuariosCoreSsoPorRfQuery request, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var resposta = await httpClient
                .GetAsync(string.Format(ServicosEolConstants.URL_USUARIOS_CORESSO_POR_LOGIN, request.CodigoRf), cancellationToken);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<UsuarioCoreSsoDto>(json);
            }
            return null;
        }
    }
}