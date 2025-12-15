using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;

namespace SME.SGP.Aplicacao
{
    public interface IObterHistoricosDeAlteracoesNovoEncaminhamentoNAAPAUseCase : IUseCase<long, PaginacaoResultadoDto<NovoEncaminhamentoNAAPAHistoricoDeAlteracaoDto>>
    {
    }
}
