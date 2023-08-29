using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaAnoCommandHandler : IRequestHandler<ConciliacaoFrequenciaAnoCommand, bool>
    {
        private readonly IMediator mediator;

        public ConciliacaoFrequenciaAnoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ConciliacaoFrequenciaAnoCommand request, CancellationToken cancellationToken)
        {
            var dresIds = await mediator.Send(ObterIdsDresQuery.Instance, cancellationToken);

            foreach (var dreId in dresIds)
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmaDreSync,
                    new ConciliacaoFrequenciaTurmaDreSyncDto(dreId, request.AnoLetivo)), cancellationToken);
            }

            return await Task.FromResult(true);
        }
    }
}
