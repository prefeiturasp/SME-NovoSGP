using MediatR;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtribuirResponsavelPlanoAEEUseCase : AbstractUseCase, IAtribuirResponsavelPlanoAEEUseCase
    {
        public AtribuirResponsavelPlanoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long planoAEEId, string responsavelRF)
                => await mediator.Send(new AtribuirResponsavelPlanoAEECommand(planoAEEId, responsavelRF));
    }
}
