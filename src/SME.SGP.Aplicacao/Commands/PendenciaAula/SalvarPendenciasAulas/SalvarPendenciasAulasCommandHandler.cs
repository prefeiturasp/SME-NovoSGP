using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciasAulasCommandHandler : IRequestHandler<SalvarPendenciasAulasCommand, bool>
    {
        private readonly IRepositorioPendenciaAula repositorioPendenciaAula;

        public SalvarPendenciasAulasCommandHandler(IRepositorioPendenciaAula repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<bool> Handle(SalvarPendenciasAulasCommand request, CancellationToken cancellationToken)
        {
            repositorioPendenciaAula.SalvarVarias(request.PendenciaId, request.AulasIds);
            return true;
        }
    }
}
