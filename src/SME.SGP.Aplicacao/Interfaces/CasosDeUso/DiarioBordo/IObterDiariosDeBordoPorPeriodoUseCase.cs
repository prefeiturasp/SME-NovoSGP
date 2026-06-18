using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterDiariosDeBordoPorPeriodoUseCase : IUseCase<FiltroTurmaComponentePeriodoDto, PaginacaoResultadoDto<DiarioBordoDevolutivaDto>>
    {
    }
}
