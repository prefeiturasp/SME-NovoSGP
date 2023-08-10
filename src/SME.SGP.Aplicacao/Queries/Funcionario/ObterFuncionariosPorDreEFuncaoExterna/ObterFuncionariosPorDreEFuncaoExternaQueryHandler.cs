using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    internal class ObterFuncionariosPorDreEFuncaoExternaQueryHandler : IRequestHandler<ObterFuncionariosPorDreEFuncaoExternaQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterFuncionariosPorDreEFuncaoExternaQueryHandler(IHttpClientFactory httpClientFactory, 
            IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));            
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionariosPorDreEFuncaoExternaQuery request, CancellationToken cancellationToken)
        {
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance, cancellationToken);

            using (var httpClient = httpClientFactory.CreateClient("servicoEOL"))
            {
                var resposta = await httpClient.GetAsync($@"funcionarios/perfis/{usuario.PerfilAtual}/dres/{request.CodigoDRE}", cancellationToken);

                if (resposta.IsSuccessStatusCode)
                {
                    var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
                    var listaRetorno = JsonConvert.DeserializeObject<IEnumerable<UsuarioEolRetornoDto>>(json);
                    return listaRetorno.Where(c => c.CodigoFuncaoExterna == request.CodigoFuncaoExterna).OrderBy(a => a.NomeServidor);
                }
            }

            return null;
        }
    }
}