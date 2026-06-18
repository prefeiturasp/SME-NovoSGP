using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiariosDeBordoPorPeriodoUseCase : AbstractUseCase, IObterDiariosDeBordoPorPeriodoUseCase
    {
        public ObterDiariosDeBordoPorPeriodoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>> Executar(FiltroTurmaComponentePeriodoDto param)
            => await mediator.Send(new ObterDiariosDeBordoPorPeriodoQuery(param.TurmaCodigo, param.ComponenteCurricularCodigo, param.PeriodoInicio, param.PeriodoFim));
    }
}
