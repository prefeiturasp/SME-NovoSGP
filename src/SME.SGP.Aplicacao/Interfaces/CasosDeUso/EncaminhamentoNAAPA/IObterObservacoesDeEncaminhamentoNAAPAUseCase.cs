using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterObservacoesDeEncaminhamentoNAAPAUseCase : IUseCase<long, PaginacaoResultadoDto<AtendimentoNAAPAObservacoesDto>>
    {
    }
}
