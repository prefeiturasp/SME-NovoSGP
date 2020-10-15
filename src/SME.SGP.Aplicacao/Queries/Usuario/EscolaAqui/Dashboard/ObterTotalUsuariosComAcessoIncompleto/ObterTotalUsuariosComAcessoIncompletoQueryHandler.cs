using MediatR;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalUsuariosComAcessoIncompletoQueryHandler : IRequestHandler<ObterTotalUsuariosComAcessoIncompletoQuery, long>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterTotalUsuariosComAcessoIncompletoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<long> Handle(ObterTotalUsuariosComAcessoIncompletoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoAcompanhamentoEscolar");

            var url = new StringBuilder(@"/api/v1/dashboard/adesao/usuarios/incompletos");

            if (!String.IsNullOrEmpty(request.CodigoDre))
                url.Append(@"?codigoDre=" + request.CodigoDre);

            if (!String.IsNullOrEmpty(request.CodigoDre) && request.CodigoUe > 0)
                url.Append(@"&codigoUe=" + request.CodigoUe);

            var resposta = await httpClient.GetAsync($"{url}");

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<long>(json);
            }

            return 0;
        }
    }
}
