using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterDataDiarioBordoSemDevolutivaPorTurmaComponenteUseCase : AbstractUseCase, IObterDataDiarioBordoSemDevolutivaPorTurmaComponenteUseCase
    {
        public ObterDataDiarioBordoSemDevolutivaPorTurmaComponenteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<DateTime?> Executar(FiltroTurmaComponenteDto param)
            => await mediator.Send(new ObterDataDiarioBordoSemDevolutivaPorTurmaComponenteQuery(param.TurmaCodigo, param.ComponenteCurricularCodigo));
    }
}
