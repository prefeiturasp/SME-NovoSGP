using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosTotaisUseCase : IObterComunicadosTotaisUseCase
    {
        private readonly IMediator mediator;

        public ObterComunicadosTotaisUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<ComunicadosTotaisResultado> Executar(int anoLetivo, string codigoDre, string codigoUe)
        {
            return await mediator.Send(new ObterComunicadosTotaisQuery(anoLetivo, codigoDre, codigoUe));
        }
    }
}
