using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterAtendimentoNAAPAUseCase : IUseCase<FiltroAtendimentoNAAPADto, PaginacaoResultadoDto<AtendimentoNAAPAResumoDto>>
    {
        
    }
}
