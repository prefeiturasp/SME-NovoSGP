using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterEncaminhamentosAEEUseCase : IUseCase<FiltroPesquisaEncaminhamentosAEEDto, PaginacaoResultadoDto<EncaminhamentoAEEResumoDto>>
    {
    }
}
