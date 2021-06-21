using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardRegistrosIndividuaisUseCase : AbstractUseCase, IObterDadosDashboardRegistrosIndividuaisUseCase
    {
        public ObterDadosDashboardRegistrosIndividuaisUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroDasboardRegistroIndividualDTO filtro)
        {
            if (filtro.UeId > 0)
            {
                return await mediator.Send(new ObterMediaRegistrosIndividuaisPorTurmaQuery(filtro.AnoLetivo, filtro.DreId, filtro.UeId, filtro.Modalidade));
            }
            else
                return await mediator.Send(new ObterMediaRegistrosIndividuaisPorAnoQuery(filtro.AnoLetivo, filtro.DreId, filtro.Modalidade));
        }
    }
}
