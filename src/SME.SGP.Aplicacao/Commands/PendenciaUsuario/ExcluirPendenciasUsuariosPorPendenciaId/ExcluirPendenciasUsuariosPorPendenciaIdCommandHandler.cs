using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciasUsuariosPorPendenciaIdCommandHandler : IRequestHandler<ExcluirPendenciasUsuariosPorPendenciaIdCommand, bool>
    {
        private readonly IRepositorioPendenciaUsuario repositorioPendenciaUsuario;

        public ExcluirPendenciasUsuariosPorPendenciaIdCommandHandler(IRepositorioPendenciaUsuario repositorioPendenciaUsuario)
        {
            this.repositorioPendenciaUsuario = repositorioPendenciaUsuario ?? throw new ArgumentNullException(nameof(repositorioPendenciaUsuario));
        }

        public async Task<bool> Handle(ExcluirPendenciasUsuariosPorPendenciaIdCommand request, CancellationToken cancellationToken)
        {
            await repositorioPendenciaUsuario.ExcluirPorPendenciaId(request.PendenciaId);
            return true;
        }
    }
}
