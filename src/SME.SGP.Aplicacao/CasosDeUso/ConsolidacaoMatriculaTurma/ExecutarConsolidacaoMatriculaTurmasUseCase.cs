using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoMatriculaTurmasUseCase : AbstractUseCase, IExecutarConsolidacaoMatriculaTurmasUseCase
    {
        public ExecutarConsolidacaoMatriculaTurmasUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExecutarConsolidacaoFrequenciaTurmaSyncUseCase", "Rabbit - ExecutarConsolidacaoFrequenciaTurmaSyncUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidacaoMatriculasTurmasDreCarregar, string.Empty, Guid.NewGuid(), null));
        }
    }
}
