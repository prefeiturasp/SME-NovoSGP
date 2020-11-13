using MediatR;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalUsuariosValidosUseCase : IObterTotalUsuariosValidosUseCase
    {
        private readonly IMediator mediator;

        public ObterTotalUsuariosValidosUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<long> Executar(string codigoDre, long codigoUe)
        {
            return await mediator.Send(new ObterTotalUsuariosValidosQuery(codigoDre, codigoUe));
        }
    }
}
