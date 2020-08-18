using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimaDataDevolutivaPorTurmaComponenteUseCase : AbstractUseCase, IObterUltimaDataDevolutivaPorTurmaComponenteUseCase
    {
        public ObterUltimaDataDevolutivaPorTurmaComponenteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<DateTime> Executar(FiltroTurmaComponenteDto param)
            => await mediator.Send(new ObterUltimaDataDevolutivaPorTurmaComponenteQuery(param.TurmaCodigo, param.ComponenteCurricularCodigo));
    }
}
