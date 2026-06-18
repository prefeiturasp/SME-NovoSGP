using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterListaDevolutivasPorTurmaComponenteUseCase : IUseCase<FiltroListagemDevolutivaDto, PaginacaoResultadoDto<DevolutivaResumoDto>>
    {
    }
}
