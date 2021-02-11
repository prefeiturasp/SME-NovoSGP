using MediatR;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtribuirResponsavelEncaminhamentoAEEUseCase : AbstractUseCase, IAtribuirResponsavelEncaminhamentoAEEUseCase
    {
        public AtribuirResponsavelEncaminhamentoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long encaminhamentoId, string rfResponsavel)
                => await mediator.Send(new AtribuirResponsavelEncaminhamentoAEECommand(encaminhamentoId, rfResponsavel));
    }
}
