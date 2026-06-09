using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaPerfilUsuarioCommandHandler : IRequestHandler<SalvarPendenciaPerfilUsuarioCommand>
    {
        private readonly IRepositorioPendenciaPerfilUsuario repositorio;

        public SalvarPendenciaPerfilUsuarioCommandHandler(IRepositorioPendenciaPerfilUsuario repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task Handle(SalvarPendenciaPerfilUsuarioCommand request, CancellationToken cancellationToken)
        {
            await repositorio.SalvarAsync(new PendenciaPerfilUsuario(request.PendenciaPerfilId,
                                                                     request.UsuarioId,
                                                                     request.PerfilCodigo));
        }
    }
}
