using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaTurmaUeCommandHandler : IRequestHandler<ConciliacaoFrequenciaTurmaUeCommand, bool>
    {
        private readonly IMediator mediator;

        public ConciliacaoFrequenciaTurmaUeCommandHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Handle(ConciliacaoFrequenciaTurmaUeCommand request, CancellationToken cancellationToken)
        {
            var turmasCodigos = await mediator.Send(new ObterCodigosTurmasPorUeAnoQuery(request.AnoLetivo, request.UeCodigo), cancellationToken);
            var meses = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            foreach (var turmaCodigo in turmasCodigos)
            {
                foreach (var mes in meses)
                    await mediator.Send(new IncluirFilaConciliacaoFrequenciaTurmaMesCommand(turmaCodigo, mes), cancellationToken);
            }

            return await Task.FromResult(true);
        }
    }
}
