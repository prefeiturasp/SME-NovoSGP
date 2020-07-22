using MediatR;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorTurmaECodigoUeUseCase : IObterComponentesCurricularesPorTurmaECodigoUeUseCase
    {
        private readonly IMediator mediator;

        public ObterComponentesCurricularesPorTurmaECodigoUeUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ComponenteCurricularDto>> Executar(FiltroComponentesCurricularesPorTurmaECodigoUeDto filtroComponentesCurricularesPorTurmaECodigoUeDto)
        {
            var result = await mediator.Send(new ObterComponentesCurricularesPorTurmaECodigoUeQuery(filtroComponentesCurricularesPorTurmaECodigoUeDto.CodigosDeTurmas, filtroComponentesCurricularesPorTurmaECodigoUeDto.CodigoUe));
            return result;
        }
    }
}
