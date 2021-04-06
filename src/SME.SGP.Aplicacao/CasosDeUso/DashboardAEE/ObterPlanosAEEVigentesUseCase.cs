using MediatR;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAEEVigentesUseCase : AbstractUseCase, IObterPlanosAEEVigentesUseCase
    {
        public ObterPlanosAEEVigentesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AEETurmaDto>> Executar(FiltroDashboardAEEDto param)
        {
            if (param.AnoLetivo == 0)
                param.AnoLetivo = DateTime.Now.Year;
            return await mediator.Send(new ObterPlanosAEEVigentesQuery(param.AnoLetivo, param.DreId, param.UeId));
        }
    }
}
