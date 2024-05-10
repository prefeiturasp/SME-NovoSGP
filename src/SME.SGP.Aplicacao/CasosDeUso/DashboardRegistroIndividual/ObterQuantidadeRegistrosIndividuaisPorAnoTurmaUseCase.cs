using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeRegistrosIndividuaisPorAnoTurmaUseCase : AbstractUseCase, IObterQuantidadeRegistrosIndividuaisPorAnoTurmaUseCase
    {
        public ObterQuantidadeRegistrosIndividuaisPorAnoTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroDasboardRegistroIndividualDTO filtro)
            => await mediator.Send(new ObterQuantidadeRegistrosIndividuaisPorAnoTurmaQuery(filtro.AnoLetivo,
                                                                                           filtro.DreId,
                                                                                           filtro.UeId,
                                                                                           filtro.Modalidade));
    }
}
