using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaUsuarioPorPendenciaIdEUsuarioIdCommandHandler : IRequestHandler<ExcluirPendenciaUsuarioPorPendenciaIdEUsuarioIdCommand, bool>
    {
        private readonly IRepositorioPendenciaUsuario repositorioPendenciaUsuario;

        public ExcluirPendenciaUsuarioPorPendenciaIdEUsuarioIdCommandHandler(IRepositorioPendenciaUsuario repositorioPendenciaUsuario)
        {
            this.repositorioPendenciaUsuario = repositorioPendenciaUsuario ?? throw new ArgumentNullException(nameof(repositorioPendenciaUsuario));
        }

        public async Task<bool> Handle(ExcluirPendenciaUsuarioPorPendenciaIdEUsuarioIdCommand request, CancellationToken cancellationToken)
        {
            await repositorioPendenciaUsuario.ExcluirPorPendenciaIdEUsuario(request.PendenciaId, request.UsuarioId);
            return true;
        }
    }
}
