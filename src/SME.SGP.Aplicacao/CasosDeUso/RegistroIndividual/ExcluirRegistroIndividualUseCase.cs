using MediatR;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRegistroIndividualUseCase : IExcluirRegistroIndividualUseCase
    {
        private readonly IMediator mediator;

        public ExcluirRegistroIndividualUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(long registroIndividualId)
        {
            return await mediator.Send(new ExcluirRegistroIndividualCommand(registroIndividualId));
        }
    }
}
