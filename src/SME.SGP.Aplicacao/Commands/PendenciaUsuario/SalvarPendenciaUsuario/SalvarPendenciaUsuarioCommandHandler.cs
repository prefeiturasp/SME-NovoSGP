using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaUsuarioCommandHandler : IRequestHandler<SalvarPendenciaUsuarioCommand, bool>
    {
        private readonly IRepositorioPendenciaUsuario repositorioPendenciaUsuario;

        public SalvarPendenciaUsuarioCommandHandler(IRepositorioPendenciaUsuario repositorioPendenciaUsuario)
        {
            this.repositorioPendenciaUsuario = repositorioPendenciaUsuario;
        }

        public async Task<bool> Handle(SalvarPendenciaUsuarioCommand request, CancellationToken cancellationToken)
        {
            await repositorioPendenciaUsuario.SalvarAsync(new Dominio.PendenciaUsuario
            {
                UsuarioId = request.UsuarioId,
                PendenciaId = request.PendenciaId
            });
            return true;
        }
    }
}
