using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioLogadoIdQueryHandler : IRequestHandler<ObterUsuarioLogadoIdQuery, long>
    {
        private readonly IContextoAplicacao contextoAplicacao;
        private readonly IRepositorioUsuario repositorioUsuario;

        public ObterUsuarioLogadoIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioUsuario repositorioUsuario)
        {
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
        }
        public string ObterLoginAtual()
        {
            var loginAtual = contextoAplicacao.ObterVarivel<string>("login");
            if (loginAtual == null)
                throw new NegocioException("Não foi possível localizar o login no token");

            return loginAtual;
        }
        public Task<long> Handle(ObterUsuarioLogadoIdQuery request, CancellationToken cancellationToken)
        {
            var login = ObterLoginAtual();
            if (string.IsNullOrWhiteSpace(login))
                throw new NegocioException("Usuário não encontrado.");

            var usuario = repositorioUsuario.ObterPorCodigoRfLogin(string.Empty, login);

            if (usuario == null)
            {
                throw new NegocioException("Usuário não encontrado.");
            }

            return Task.FromResult(usuario.Id);

        }
    }
}
