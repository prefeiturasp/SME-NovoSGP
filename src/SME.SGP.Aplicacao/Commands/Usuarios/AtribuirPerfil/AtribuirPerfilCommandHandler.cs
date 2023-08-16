using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtribuirPerfilCommandHandler : IRequestHandler<AtribuirPerfilCommand>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public AtribuirPerfilCommandHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<Unit> Handle(AtribuirPerfilCommand request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_PERFIS_SERVIDORES_PERFIL_ATRIBUIR_PERFIL, request.CodigoRf, request.Perfil));

            if (resposta.IsSuccessStatusCode)
                return Unit.Value;

            var mensagem = await resposta.Content.ReadAsStringAsync();

            throw new NegocioException(mensagem);
        }
    }
}
