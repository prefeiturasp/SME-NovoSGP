using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaCalcularFrequenciaPorTurmaCommandHandler : IRequestHandler<IncluirFilaCalcularFrequenciaPorTurmaCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaCalcularFrequenciaPorTurmaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaCalcularFrequenciaPorTurmaCommand request, CancellationToken cancellationToken)
        {
            var comando = new CalcularFrequenciaPorTurmaCommand(request.Alunos, request.DataAula, request.TurmaId, request.DisciplinaId);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaCalculoFrequenciaPorTurmaComponente, comando, Guid.NewGuid(), null));

            return true;
        }
    }
}