using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPorRfOuCriaQueryHandler : IRequestHandler<ObterUsuarioPorRfOuCriaQuery, Usuario>
    {
        private readonly IRepositorioUsuarioConsulta repositorioUsuario;

        public ObterUsuarioPorRfOuCriaQueryHandler(IRepositorioUsuarioConsulta repositorioUsuario)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
        }

        public async Task<Usuario> Handle(ObterUsuarioPorRfOuCriaQuery request, CancellationToken cancellationToken)
        {
            var usuario = await repositorioUsuario.ObterUsuarioPorCodigoRfAsync(request.UsuarioRf);
            if (usuario == null)
            {
                usuario = new Usuario() { CodigoRf = request.UsuarioRf, Login = request.UsuarioRf };
                await repositorioUsuario.SalvarAsync(usuario);
            }

            return usuario;
        }
    }
}
