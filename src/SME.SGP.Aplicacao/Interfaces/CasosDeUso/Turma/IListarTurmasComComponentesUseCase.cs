using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IListarTurmasComComponentesUseCase : IUseCase<FiltroTurmaDto, PaginacaoResultadoDto<TurmaComComponenteDto>>
    {
    }
}
