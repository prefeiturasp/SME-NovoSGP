using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterOuAdicionarUsuarioIdCommandHandler : IRequestHandler<ObterOuAdicionarUsuarioIdCommand, long>
    {
        private readonly IRepositorioUsuario repositorioUsuario;

        public ObterOuAdicionarUsuarioIdCommandHandler(IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
        }
        public async Task<long> Handle(ObterOuAdicionarUsuarioIdCommand request, CancellationToken cancellationToken)
        {
            var usuario = repositorioUsuario.ObterPorCodigoRfLogin(request.UsuarioRF, string.Empty);

            if (usuario == null)
            {
                var usuarioParaAdicionar = new Usuario() { CodigoRf = request.UsuarioRF, Login = request.UsuarioRF, Nome = request.UsuarioNome };
                return await repositorioUsuario.SalvarAsync(usuarioParaAdicionar);
            }
            else return usuario.Id;

        }
    }
}
