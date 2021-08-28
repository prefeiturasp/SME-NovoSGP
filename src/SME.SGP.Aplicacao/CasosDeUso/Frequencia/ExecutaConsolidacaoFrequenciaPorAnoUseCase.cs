using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaConsolidacaoFrequenciaPorAnoUseCase : AbstractUseCase, IExecutaConsolidacaoFrequenciaPorAnoUseCase
    {
        public ExecutaConsolidacaoFrequenciaPorAnoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar(int ano)
        {
            await mediator.Send(new LimparConsolidacaoFrequenciaTurmasPorAnoCommand(ano));
            await mediator.Send(new ExecutarConsolidacaoFrequenciaNoAnoCommand(ano));
        }
    }
}
