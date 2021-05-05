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

        public async Task<IEnumerable<GraficoAusenciasComJustificativaResultadoDto>> Executar(int anoLetivo, long dreId, long ueId, Modalidade modalidade, int semestre)
        {
            return await mediator.Send(new ObterDadosDashboardAusenciasComJustificativaQuery(anoLetivo, dreId, ueId, modalidade, semestre));
        }
    }
}
