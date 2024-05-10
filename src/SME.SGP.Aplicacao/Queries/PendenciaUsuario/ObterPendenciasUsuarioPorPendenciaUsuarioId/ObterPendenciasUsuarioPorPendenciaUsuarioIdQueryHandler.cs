using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasUsuarioPorPendenciaUsuarioIdQueryHandler : IRequestHandler<ObterPendenciasUsuarioPorPendenciaUsuarioIdQuery, bool>
    {
        private readonly IRepositorioPendenciaUsuarioConsulta repositorioPendenciaUsuario;

        public ObterPendenciasUsuarioPorPendenciaUsuarioIdQueryHandler(IRepositorioPendenciaUsuarioConsulta repositorioPendenciaUsuario)
        {
            this.repositorioPendenciaUsuario = repositorioPendenciaUsuario;
        }

        public async Task<bool> Handle(ObterPendenciasUsuarioPorPendenciaUsuarioIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPendenciaUsuario.ObterPendenciasUsuarioPorPendenciaUsuarioId(request.UsuarioId, request.PendenciaId);
        }
    }
}
