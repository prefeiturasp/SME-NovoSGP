using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPorIdQueryHandler : IRequestHandler<ObterUsuarioPorIdQuery, Dominio.Usuario>
    {
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IMediator mediator;
        private readonly IRepositorioPrioridadePerfil repositorioPrioridadePerfil;

        public ObterUsuarioPorIdQueryHandler(IRepositorioUsuario repositorioUsuario, IMediator mediator, IRepositorioPrioridadePerfil repositorioPrioridadePerfil)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioPrioridadePerfil = repositorioPrioridadePerfil ?? throw new ArgumentNullException(nameof(repositorioPrioridadePerfil));
        }
        public async Task<Dominio.Usuario> Handle(ObterUsuarioPorIdQuery request, CancellationToken cancellationToken)
        {
            var usuario = await repositorioUsuario.ObterPorIdAsync(request.Id);

            var perfisDoUsuario = await ObterPerfisUsuario(usuario.Login);
            usuario.DefinirPerfis(perfisDoUsuario);

            return usuario;
        }

        public async Task<IEnumerable<PrioridadePerfil>> ObterPerfisUsuario(string login)
        {
            var perfisPorLogin = await mediator.Send(new ObterPerfisPorLoginQuery(login));

            if (perfisPorLogin.EhNulo())
                throw new NegocioException($"Não foi possível obter os perfis do usuário {login}");

            var perfisDoUsuario = repositorioPrioridadePerfil.ObterPerfisPorIds(perfisPorLogin.Perfis);

            return perfisDoUsuario;
        }
    }
}
