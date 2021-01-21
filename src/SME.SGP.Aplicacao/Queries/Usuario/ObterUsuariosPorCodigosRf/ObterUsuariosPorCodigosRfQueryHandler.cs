using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuariosPorCodigosRfQueryHandler : IRequestHandler<ObterUsuariosPorCodigosRfQuery, IEnumerable<Usuario>>
    {
        private readonly IRepositorioUsuario repositorioUsuario;

        public ObterUsuariosPorCodigosRfQueryHandler(IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
        }

        public Task<IEnumerable<Usuario>> Handle(ObterUsuariosPorCodigosRfQuery request, CancellationToken cancellationToken)
            => repositorioUsuario.ObterUsuariosPorCodigoRf(request.CodigosRf);
    }
}
