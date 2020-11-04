using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui.DashboardAdesao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotaisAdesaoAgrupadosPorDreUseCase : IObterTotaisAdesaoAgrupadosPorDreUseCase
    {
        private readonly IMediator mediator;

        public ObterTotaisAdesaoAgrupadosPorDreUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<TotaisAdesaoAgrupadoProDreResultado>> Executar()
        {
            return await mediator.Send(new ObterTotaisAdesaoAgrupadosPorDreQuery());
        }
    }
}
