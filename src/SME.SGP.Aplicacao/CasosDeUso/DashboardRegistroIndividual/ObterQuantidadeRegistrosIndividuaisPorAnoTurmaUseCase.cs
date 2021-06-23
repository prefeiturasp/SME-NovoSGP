using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeRegistrosIndividuaisPorAnoTurmaUseCase : IObterQuantidadeRegistrosIndividuaisPorAnoTurmaUseCase
    {
        private readonly IMediator mediator;

        public ObterQuantidadeRegistrosIndividuaisPorAnoTurmaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroDasboardRegistroIndividualDTO filtro)
        {
            return await mediator.Send(new ObterQuantidadeRegistrosIndividuaisPorAnoTurmaQuery(filtro.AnoLetivo, filtro.DreId, filtro.UeId, filtro.Modalidade));
        }
    }
}
