using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverResponsavelPlanoAEEUseCase : AbstractUseCase, IRemoverResponsavelPlanoAEEUseCase
    {
        public RemoverResponsavelPlanoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long planoAeeId)
            => await mediator.Send(new RemoverResponsavelPlanoAEECommand(planoAeeId));
    }
}
