using MediatR;
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
        private readonly IServicoEol servicoEOL;
        private readonly IRepositorioPrioridadePerfil repositorioPrioridadePerfil;

        public ObterUsuariosPorRfOuCriaQueryHandler(IRepositorioUsuario repositorioUsuario, IServicoEol servicoEOL, IRepositorioPrioridadePerfil repositorioPrioridadePerfil)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.repositorioPrioridadePerfil = repositorioPrioridadePerfil ?? throw new ArgumentNullException(nameof(repositorioPrioridadePerfil));
        }

        public async Task<IEnumerable<Usuario>> Handle(ObterUsuariosPorRfOuCriaQuery request, CancellationToken cancellationToken)
        {
            var usuarios = (await repositorioUsuario.ObterUsuariosPorCodigoRf(request.CodigosRf))?.ToList();

            var usuariosFaltantesRf = request.CodigosRf.Where(u => !usuarios.Select(us => us.CodigoRf).Contains(u));

            if (usuariosFaltantesRf != null && usuariosFaltantesRf.Any())
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
            var perfisPorLogin = await servicoEOL.ObterPerfisPorLogin(login);

            if (perfisPorLogin == null)
                throw new NegocioException($"Não foi possível obter os perfis do usuário {login}");

            var perfisDoUsuario = repositorioPrioridadePerfil.ObterPerfisPorIds(perfisPorLogin.Perfis);

            return perfisDoUsuario;
        }
    }
}
