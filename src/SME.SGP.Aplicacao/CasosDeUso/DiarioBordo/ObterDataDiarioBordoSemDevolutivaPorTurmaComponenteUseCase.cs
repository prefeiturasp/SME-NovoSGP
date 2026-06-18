using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

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
