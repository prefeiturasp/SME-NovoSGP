using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosTotaisAgrupadosPorDreUseCase : IObterComunicadosTotaisAgrupadosPorDreUseCase
    {
        private readonly IMediator mediator;

        public ObterComunicadosTotaisAgrupadosPorDreUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ComunicadosTotaisPorDreResultado>> Executar(int anoLetivo)
        {
            return await mediator.Send(new ObterComunicadosTotaisAgrupadosPorDreQuery(anoLetivo));
        }
    }
}
