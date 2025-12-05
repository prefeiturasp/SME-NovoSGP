using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterObservacoesDeAtendimentoNAAPAUseCase : IUseCase<long, PaginacaoResultadoDto<AtendimentoNAAPAObservacoesDto>>
    {
    }
}
