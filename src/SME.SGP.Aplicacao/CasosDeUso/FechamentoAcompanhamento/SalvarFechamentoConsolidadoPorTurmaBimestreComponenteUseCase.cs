using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarFechamentoConsolidadoPorTurmaBimestreComponenteUseCase : AbstractUseCase, ISalvarFechamentoConsolidadoPorTurmaBimestreComponenteUseCase
    {
        public SalvarFechamentoConsolidadoPorTurmaBimestreComponenteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var command = mensagemRabbit.ObterObjetoMensagem<SalvarFechamentoConsolidadoCommand>();

            return await mediator.Send(command);
        }
    }
}
