using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPorRfQueryHandler : IRequestHandler<ObterUsuarioPorRfQuery, Dominio.Usuario>
    {
        private readonly IRepositorioUsuarioConsulta repositorioUsuario;

        public ObterUsuarioPorRfQueryHandler(IRepositorioUsuarioConsulta repositorioUsuario)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
        }
        public async Task<Dominio.Usuario> Handle(ObterUsuarioPorRfQuery request, CancellationToken cancellationToken)
        {
            return await repositorioUsuario.ObterUsuarioPorCodigoRfAsync(request.CodigoRf);
        }
    }
}
