using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
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

            if (request.Meses.NaoEhNulo() && request.Meses.Any())
            {
                foreach(var mes in request.Meses)
                    await PublicarConsolidacaoFrequenciaAlunoMensal(request.TurmaId, mes, cancellationToken);
            }
            else
                await PublicarConsolidacaoFrequenciaAlunoMensal(request.TurmaId, request.DataAula.Month, cancellationToken);

            return true;
        }

        public async Task PublicarConsolidacaoFrequenciaAlunoMensal(string turmaId, int month, CancellationToken cancellationToken)
        {
            var comandoConsolidacao = new FiltroConsolidacaoFrequenciaAlunoMensal(turmaId, month);
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaConsolidacaoFrequenciaAlunoPorTurmaMensal, comandoConsolidacao, Guid.NewGuid(), null),
                cancellationToken);
        }
    }
}