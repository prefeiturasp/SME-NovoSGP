using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterEncaminhamentoNAAPAUseCase : IUseCase<FiltroAtendimentoNAAPADto, PaginacaoResultadoDto<AtendimentoNAAPAResumoDto>>
    {
        
    }
}
