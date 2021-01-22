using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarPendenciaParametroEventoCommandHandler : IRequestHandler<AtualizarPendenciaParametroEventoCommand, bool>
    {
        private readonly IRepositorioPendenciaParametroEvento repositorioPendenciaParametroEvento;

        public AtualizarPendenciaParametroEventoCommandHandler(IRepositorioPendenciaParametroEvento repositorioPendenciaParametroEvento)
        {
            this.repositorioPendenciaParametroEvento = repositorioPendenciaParametroEvento ?? throw new ArgumentNullException(nameof(repositorioPendenciaParametroEvento));
        }

        public async Task<bool> Handle(AtualizarPendenciaParametroEventoCommand request, CancellationToken cancellationToken)
        {
            await repositorioPendenciaParametroEvento.SalvarAsync(request.PendenciaParametroEvento);
            return true;
        }
    }
}
