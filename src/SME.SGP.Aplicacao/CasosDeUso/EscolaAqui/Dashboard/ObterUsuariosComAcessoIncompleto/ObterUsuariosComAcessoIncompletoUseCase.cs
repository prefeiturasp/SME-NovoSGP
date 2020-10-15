using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalUsuariosComAcessoIncompletoUseCase : IObterTotalUsuariosComAcessoIncompletoUseCase
    {
        private readonly IMediator mediator;

        public ObterTotalUsuariosComAcessoIncompletoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<long> Executar(string codigoDre, long codigoUe)
        {
            return await mediator.Send(new ObterTotalUsuariosComAcessoIncompletoQuery(codigoDre, codigoUe));
        }
    }
}
