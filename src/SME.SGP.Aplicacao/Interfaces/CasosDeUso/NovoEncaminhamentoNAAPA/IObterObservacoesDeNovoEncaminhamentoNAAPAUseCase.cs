using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterObservacoesDeNovoEncaminhamentoNAAPAUseCase : IUseCase<long, PaginacaoResultadoDto<NovoEncaminhamentoNAAPAObservacoesDto>>
    {
    }
}
