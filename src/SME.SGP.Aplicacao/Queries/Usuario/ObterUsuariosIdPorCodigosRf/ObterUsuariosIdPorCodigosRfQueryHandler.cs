using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuariosIdPorCodigosRfQueryHandler : IRequestHandler<ObterUsuariosIdPorCodigosRfQuery, IEnumerable<long>>
    {
        private readonly IRepositorioUsuarioConsulta repositorioUsuario;

        public ObterUsuariosIdPorCodigosRfQueryHandler(IRepositorioUsuarioConsulta repositorioUsuario)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
        }

        public Task<IEnumerable<long>> Handle(ObterUsuariosIdPorCodigosRfQuery request, CancellationToken cancellationToken)
                => repositorioUsuario.ObterUsuariosIdPorCodigoRf(request.CodigosRf);
    }
}
