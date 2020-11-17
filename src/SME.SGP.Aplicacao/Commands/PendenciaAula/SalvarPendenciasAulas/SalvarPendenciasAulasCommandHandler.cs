using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciasAulasCommandHandler : IRequestHandler<SalvarPendenciasAulasCommand, bool>
    {
        private readonly IRepositorioPendenciaAula repositorioPendenciaAula;
        private readonly IMediator mediator;

        public SalvarPendenciasAulasCommandHandler(IRepositorioPendenciaAula repositorioPendenciaAula, IMediator mediator)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(SalvarPendenciasAulasCommand request, CancellationToken cancellationToken)
        {
            repositorioPendenciaAula.SalvarVarias(request.PendenciaId, request.AulasIds);

            IList<long> professoresId = new List<long>();
            foreach (var aulaId in request.AulasIds)
            {
                var professor = await mediator.Send(new ObterProfessorDaTurmaPorAulaIdQuery(aulaId));

                if(professor != null)
                    await mediator.Send(new SalvarPendenciaUsuarioCommand(request.PendenciaId, professor.Id));               
            }
            return true;
        }
    }
}
