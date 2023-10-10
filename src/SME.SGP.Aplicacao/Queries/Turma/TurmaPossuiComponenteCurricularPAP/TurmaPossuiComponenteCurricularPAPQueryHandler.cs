using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class TurmaPossuiComponenteCurricularPAPQueryHandler : IRequestHandler<TurmaPossuiComponenteCurricularPAPQuery, bool>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public TurmaPossuiComponenteCurricularPAPQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<bool> Handle(TurmaPossuiComponenteCurricularPAPQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            
            var url = string.Format(ServicosEolConstants.URL_COMPONENTES_CURRICULARES_TURMAS_FUNCIONARIOS_PERFIS_VALIDAR_PAP, request.TurmaCodigo, request.Login, request.Perfil);

            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode)
                return false;

            var retorno = await resposta.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<bool>(retorno);
        }
    }
}
