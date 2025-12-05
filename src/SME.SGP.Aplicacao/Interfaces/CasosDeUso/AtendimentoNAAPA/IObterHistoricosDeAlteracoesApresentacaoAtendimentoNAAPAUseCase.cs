using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAUseCase : IUseCase<long, PaginacaoResultadoDto<AtendimentoNAAPAHistoricoDeAlteracaoDto>>
    {
    }
}
