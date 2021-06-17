using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoFrequenciaTurmaSyncUseCase : AbstractUseCase, IExecutarConsolidacaoFrequenciaTurmaSyncUseCase
    {
        public ExecutarConsolidacaoFrequenciaTurmaSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExecutarConsolidacaoFrequenciaTurmaSyncUseCase", "Rabbit - ExecutarConsolidacaoFrequenciaTurmaSyncUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidacaoFrequenciasTurmasCarregar, string.Empty, Guid.NewGuid(), null));
        }
    }
}
