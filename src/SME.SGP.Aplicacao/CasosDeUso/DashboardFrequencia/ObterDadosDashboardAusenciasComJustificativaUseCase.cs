using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardAusenciasComJustificativaUseCase : IObterDadosDashboardAusenciasComJustificativaUseCase
    {
        private readonly IMediator mediator;

        public ObterDadosDashboardAusenciasComJustificativaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<GraficoAusenciasComJustificativaPorAnoDto>> IObterDadosDashboardAusenciasComJustificativaUseCase.Executar(int anoLetivo, long dreId, long ueId, Modalidade modalidade)
        {
            return await mediator.Send(new ObterDadosDashboardAusenciasComJustificativaPorAnoQuery(anoLetivo, dreId, ueId, modalidade));
        }
    }

    
}
