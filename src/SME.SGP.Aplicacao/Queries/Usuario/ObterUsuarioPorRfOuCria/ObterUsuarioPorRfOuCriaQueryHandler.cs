using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPorRfOuCriaQueryHandler : IRequestHandler<ObterUsuarioPorRfOuCriaQuery, Usuario>
    {
        private readonly IRepositorioUsuarioConsulta repositorioUsuarioConsulta;
        private readonly IRepositorioUsuario repositorioUsuario;

        public ObterUsuarioPorRfOuCriaQueryHandler(IRepositorioUsuarioConsulta repositorioUsuarioConsulta, IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioUsuarioConsulta = repositorioUsuarioConsulta ?? throw new System.ArgumentNullException(nameof(repositorioUsuarioConsulta));
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
        }

        public async Task<Usuario> Handle(ObterUsuarioPorRfOuCriaQuery request, CancellationToken cancellationToken)
        {
            var usuario = await repositorioUsuarioConsulta.ObterUsuarioPorCodigoRfAsync(request.UsuarioRf);
            if (usuario.EhNulo())
            {
                usuario = new Usuario() { CodigoRf = request.UsuarioRf, Login = request.UsuarioRf };
                await repositorioUsuario.SalvarAsync(usuario);
            }

            return usuario;
        }
    }
}
