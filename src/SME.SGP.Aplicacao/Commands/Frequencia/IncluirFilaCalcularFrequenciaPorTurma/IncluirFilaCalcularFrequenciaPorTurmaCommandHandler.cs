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
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaCalculoFrequenciaPorTurmaComponente, comando, Guid.NewGuid(), null), 
                cancellationToken);

            var comandoConsolidacao = new FiltroConsolidacaoFrequenciaAlunoMensal(request.TurmaId, request.DataAula.Month);
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaConsolidacaoFrequenciaAlunoPorTurmaMensal, comandoConsolidacao, Guid.NewGuid(), null),
                cancellationToken);

            return true;
        }
    }
}