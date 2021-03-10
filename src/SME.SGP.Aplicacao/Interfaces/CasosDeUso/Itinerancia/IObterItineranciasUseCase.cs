using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterItineranciasUseCase : IUseCase<FiltroPesquisaItineranciasDto, PaginacaoResultadoDto<ItineranciaResumoDto>>
    {
    }
}