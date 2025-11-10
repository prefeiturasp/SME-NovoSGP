using MediatR;
using SME.SGP.Infra;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.EOL.Usuarios.ObterUsuariosCoreSsoPaginado
{
    public class ObterUsuariosCoreSsoPaginadoQueryHandler : IRequestHandler<ObterUsuariosCoreSsoPaginadoQuery, PaginacaoResultadoDto<UsuarioCoreSsoDto>>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ObterUsuariosCoreSsoPaginadoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<PaginacaoResultadoDto<UsuarioCoreSsoDto>> Handle(ObterUsuariosCoreSsoPaginadoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var url = string.Format(ServicosEolConstants.URL_USUARIOS_CORESSO_PAGINADO, request.Pagina, request.RegistrosPorPagina);
            if (!string.IsNullOrEmpty(request.Rf))
                url += $"&rf={request.Rf}";
            if (!string.IsNullOrEmpty(request.Nome))
                url += $"&nome={request.Nome}";

            var resposta = await httpClient
                .GetAsync(url, cancellationToken);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<PaginacaoResultadoDto<UsuarioCoreSsoDto>>(json);
            }
            return null;
        }
    }
}