using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaTurmaDreCommandHandler : IRequestHandler<ConciliacaoFrequenciaTurmaDreCommand, bool>
    {
        private readonly IMediator mediator;

        public ConciliacaoFrequenciaTurmaDreCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ConciliacaoFrequenciaTurmaDreCommand request, CancellationToken cancellationToken)
        {
            var uesCodigos = await mediator.Send(new ObterUesCodigosPorDreQuery(request.DreId), cancellationToken);

            foreach (var ueCodigo in uesCodigos)
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmaUeSync,
                    new ConciliacaoFrequenciaTurmaUeSyncDto(ueCodigo, request.AnoLetivo)), cancellationToken);
            }

            return await Task.FromResult(true);
        }
    }
}
