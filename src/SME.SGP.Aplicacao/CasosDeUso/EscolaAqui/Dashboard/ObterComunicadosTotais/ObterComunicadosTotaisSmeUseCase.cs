using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosTotaisSmeUseCase : IObterComunicadosTotaisSmeUseCase
    {
        private readonly IMediator mediator;

        public ObterComunicadosTotaisSmeUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<ComunicadosTotaisSmeResultado> Executar(int anoLetivo)
        {
            return await mediator.Send(new ObterComunicadosTotaisQuery(anoLetivo));
        }
    }
}
