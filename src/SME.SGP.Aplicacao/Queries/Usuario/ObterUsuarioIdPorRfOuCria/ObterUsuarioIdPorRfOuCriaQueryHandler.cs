using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioIdPorRfOuCriaQueryHandler : IRequestHandler<ObterUsuarioIdPorRfOuCriaQuery, long>
    {
        private readonly IRepositorioUsuarioConsulta repositorioUsuarioConsulta;
        private readonly IRepositorioUsuario repositorioUsuario;

        public ObterUsuarioIdPorRfOuCriaQueryHandler(IRepositorioUsuarioConsulta repositorioUsuarioConsulta, IRepositorioUsuario repositorioUsuario)
        {            
            this.repositorioUsuarioConsulta = repositorioUsuarioConsulta ?? throw new System.ArgumentNullException(nameof(repositorioUsuarioConsulta));
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
        }

        public async Task<long> Handle(ObterUsuarioIdPorRfOuCriaQuery request, CancellationToken cancellationToken)
        {
            var usuarioId = await repositorioUsuarioConsulta.ObterUsuarioIdPorLoginAsync(request.UsuarioRf);

            if (usuarioId == 0)
                return await repositorioUsuario.SalvarAsync(new Usuario() { CodigoRf = request.UsuarioRf, Login = request.UsuarioRf, Nome = request.UsuarioNome });

            return usuarioId;
        }
    }
}
