using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioFuncionarioQueryHandler : IRequestHandler<ObterUsuarioFuncionarioQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterUsuarioFuncionarioQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterUsuarioFuncionarioQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            var url = string.Format(ServicosEolConstants.URL_FUNCIONARIOS_PERFIS, usuario.PerfilAtual);
            
            url += $"?CodigoDre={request.CodigoDre}&CodigoUe={request.CodigoUe}&CodigoRf={request.CodigoRf}&NomeServidor={request.NomeServidor}";
            
            var resposta = await httpClient.GetAsync(url);

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<UsuarioEolRetornoDto>>(json);
            }
            
            return Enumerable.Empty<UsuarioEolRetornoDto>();
        }
    }
}
