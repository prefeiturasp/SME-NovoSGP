using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAUseCase : IUseCase<long, PaginacaoResultadoDto<EncaminhamentoNAAPAHistoricoDeAlteracaoDto>>
    {
    }
}
