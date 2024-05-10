using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterTurmasFechamentoAcompanhamentoUseCase : IUseCase<FiltroAcompanhamentoFechamentoTurmasDto, PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>>
    {
    }
}