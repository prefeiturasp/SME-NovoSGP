using MediatR;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverResponsavelEncaminhamentoAEEUseCase : AbstractUseCase, IRemoverResponsavelEncaminhamentoAEEUseCase
    {
        public RemoverResponsavelEncaminhamentoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long encaminhamentoId)
                => await mediator.Send(new RemoverResponsavelEncaminhamentoAEECommand(encaminhamentoId));
    }
}
