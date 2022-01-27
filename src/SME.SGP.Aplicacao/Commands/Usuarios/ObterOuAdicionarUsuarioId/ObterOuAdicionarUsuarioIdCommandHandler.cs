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
        private readonly IMediator mediator;

        public ObterOuAdicionarUsuarioIdCommandHandler(IRepositorioUsuario repositorioUsuario, IMediator mediator)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<long> Handle(ObterOuAdicionarUsuarioIdCommand request, CancellationToken cancellationToken)
        {
            var usuario = await mediator.Send(new ObterUsuarioPorCodigoRfLoginQuery(request.UsuarioRF, string.Empty));

            if (usuario == null)
            {
                var usuarioParaAdicionar = new Usuario() { CodigoRf = request.UsuarioRF, Login = request.UsuarioRF, Nome = request.UsuarioNome };
                return await repositorioUsuario.SalvarAsync(usuarioParaAdicionar);
            }
            else return usuario.Id;

        }
    }
}
