using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaConciliacaoFrequenciaTurmaCommandHandler : IRequestHandler<IncluirFilaConciliacaoFrequenciaTurmaCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaConciliacaoFrequenciaTurmaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaConciliacaoFrequenciaTurmaCommand request, CancellationToken cancellationToken)
        {
            var command = new ValidaAusenciaParaConciliacaoFrequenciaTurmaCommand(request.TurmaCodigo, request.Bimestre, request.DataInicio, request.DataFim);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaValidacaoAusenciaConciliacaoFrequenciaTurma, command, Guid.NewGuid(), null));
            SentrySdk.AddBreadcrumb($"Incluir fila de conciliação de frequência da turma [{request.TurmaCodigo}] no bimestre {request.Bimestre}", "RabbitMQ");

            return true;
        }
    }
}
