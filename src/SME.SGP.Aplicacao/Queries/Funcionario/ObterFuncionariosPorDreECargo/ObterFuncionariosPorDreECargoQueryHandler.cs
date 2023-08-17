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
    internal class ObterFuncionariosPorDreECargoQueryHandler : IRequestHandler<ObterFuncionariosPorDreECargoQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterFuncionariosPorDreECargoQueryHandler(IHttpClientFactory httpClientFactory, 
            IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));            
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionariosPorDreECargoQuery request, CancellationToken cancellationToken)
        {
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance, cancellationToken);

            using (var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO))
            {
                var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_FUNCIONARIOS_PERFIS_DRES, usuario.PerfilAtual, request.CodigoDRE) $@"{(request.CodigoCargo > 0 ? $"&codigoFuncaoAtividade={request.CodigoCargo}" : string.Empty)}", cancellationToken);

                if (resposta.IsSuccessStatusCode)
                {
                    var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
                    var listaRetorno = JsonConvert.DeserializeObject<IEnumerable<UsuarioEolRetornoDto>>(json);
                    return listaRetorno.Where(c => c.CodigoFuncaoAtividade == request.CodigoCargo).OrderBy(a => a.NomeServidor);
                }
            }

            return null;
        }
    }
}