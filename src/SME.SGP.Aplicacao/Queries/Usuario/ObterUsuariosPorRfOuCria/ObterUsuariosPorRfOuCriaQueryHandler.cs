﻿using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuariosPorRfOuCriaQueryHandler : IRequestHandler<ObterUsuariosPorRfOuCriaQuery, IEnumerable<Usuario>>
    {
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IRepositorioPrioridadePerfil repositorioPrioridadePerfil;
        private readonly IMediator mediator;

        public ObterUsuariosPorRfOuCriaQueryHandler(IRepositorioUsuario repositorioUsuario,IRepositorioPrioridadePerfil repositorioPrioridadePerfil, IMediator mediator)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
            this.repositorioPrioridadePerfil = repositorioPrioridadePerfil ?? throw new ArgumentNullException(nameof(repositorioPrioridadePerfil));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<Usuario>> Handle(ObterUsuariosPorRfOuCriaQuery request, CancellationToken cancellationToken)
        {
            var retornoUsuarios = await mediator.Send(new ObterUsuariosPorCodigosRfQuery(request.CodigosRf));

            var usuarios = retornoUsuarios.ToList();

            var usuariosFaltantesRf = request.CodigosRf.Where(u => !usuarios.Select(us => us.CodigoRf).Contains(u));

            if (usuariosFaltantesRf.NaoEhNulo() && usuariosFaltantesRf.Any())
            {
                foreach (var usuarioFaltandoRf in usuariosFaltantesRf)
                {
                    var usuarioParaInserir = new Usuario() { CodigoRf = usuarioFaltandoRf, Login = usuarioFaltandoRf };
                    usuarioParaInserir.Id = await repositorioUsuario.SalvarAsync(usuarioParaInserir);

                    usuarios.Add(usuarioParaInserir);
                }
            }

            if (request.ObterPerfis)
            {
                foreach (var usuario in usuarios)
                {
                    var perfisDoUsuario = await ObterPerfisUsuario(usuario.Login);
                    usuario.DefinirPerfis(perfisDoUsuario);
                }
            }

            return usuarios;
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
