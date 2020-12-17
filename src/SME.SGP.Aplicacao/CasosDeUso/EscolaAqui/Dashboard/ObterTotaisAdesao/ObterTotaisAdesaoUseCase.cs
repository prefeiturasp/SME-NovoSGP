using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui.DashboardAdesao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotaisAdesaoUseCase : IObterTotaisAdesaoUseCase
    {
        private readonly IMediator mediator;

        public ObterTotaisAdesaoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<TotaisAdesaoResultado>> Executar(string codigoDre, string codigoUe)
        {
            return await mediator.Send(new ObterTotaisAdesaoQuery(codigoDre, codigoUe));
        }
    }
}
