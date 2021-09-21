using MediatR;
using SME.SGP.Dominio;
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
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"perfis/servidores/{request.CodigoRf}/perfil/{request.Perfil}/atribuirPerfil");

            if (resposta.IsSuccessStatusCode)
                return Unit.Value;

            var mensagem = await resposta.Content.ReadAsStringAsync();

            throw new NegocioException(mensagem);
        }
    }
}
