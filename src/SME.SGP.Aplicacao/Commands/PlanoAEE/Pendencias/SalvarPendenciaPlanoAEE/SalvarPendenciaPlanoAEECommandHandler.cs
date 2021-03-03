using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaPlanoAEECommandHandler : IRequestHandler<SalvarPendenciaPlanoAEECommand, bool>
    {
        private readonly IRepositorioPendenciaPlanoAEE repositorioPendenciaPlanoAEE;

        public SalvarPendenciaPlanoAEECommandHandler(IMediator mediator, IRepositorioPendenciaPlanoAEE repositorioPendenciaPlanoAEE)
        {
            this.repositorioPendenciaPlanoAEE = repositorioPendenciaPlanoAEE;
        }

        public async Task<bool> Handle(SalvarPendenciaPlanoAEECommand request, CancellationToken cancellationToken)
        {
            await repositorioPendenciaPlanoAEE.SalvarAsync(new Dominio.PendenciaPlanoAEE
            {
                PlanoAEEId = request.PlanoAEEId,
                PendenciaId = request.PendenciaId   
            });
            return true;
        }
    }
}
