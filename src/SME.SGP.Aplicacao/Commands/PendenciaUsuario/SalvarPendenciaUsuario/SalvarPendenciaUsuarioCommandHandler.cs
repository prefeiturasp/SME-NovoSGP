using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaUsuarioCommandHandler : IRequestHandler<SalvarPendenciaUsuarioCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPendenciaUsuario repositorioPendenciaUsuario;

        public SalvarPendenciaUsuarioCommandHandler(IMediator mediator, IRepositorioPendenciaUsuario repositorioPendenciaUsuario)
        {
            this.mediator = mediator;
            this.repositorioPendenciaUsuario = repositorioPendenciaUsuario;
        }

        public async Task<bool> Handle(SalvarPendenciaUsuarioCommand request, CancellationToken cancellationToken)
        {
            //await repositorioPendenciaUsuario.SalvarVarias(request.PendenciaId, request.UsuarioId);
            await repositorioPendenciaUsuario.SalvarAsync(new Dominio.PendenciaUsuario
            {
                UsuarioId = request.UsuarioId,
                PendenciaId = request.PendenciaId   
            });
            return true;
        }
    }
}
