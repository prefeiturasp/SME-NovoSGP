using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.EOL.ObterGruposDeUsuarios
{
    public class ObterGruposDeUsuariosQueryHandler : IRequestHandler<ObterGruposDeUsuariosQuery, IEnumerable<GruposDeUsuariosDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterGruposDeUsuariosQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<GruposDeUsuariosDto>> Handle(ObterGruposDeUsuariosQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_GRUPOS_USUARIOS, request.TipoPerfil), cancellationToken);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
                return JsonConvert.DeserializeObject<IEnumerable<GruposDeUsuariosDto>>(json);
            }

            return Enumerable.Empty<GruposDeUsuariosDto>();
        }
    }
}
