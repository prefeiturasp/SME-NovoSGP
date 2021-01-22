using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterListagemDiariosDeBordoPorPeriodoUseCase : IUseCase<FiltroListagemDiarioBordoDto, PaginacaoResultadoDto<DiarioBordoTituloDto>>
    {
    }
}
