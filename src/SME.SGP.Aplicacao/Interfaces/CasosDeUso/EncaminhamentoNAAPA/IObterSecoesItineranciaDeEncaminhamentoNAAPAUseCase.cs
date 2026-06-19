using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterSecoesItineranciaDeEncaminhamentoNAAPAUseCase : IUseCase<long, PaginacaoResultadoDto<EncaminhamentoNAAPASecaoItineranciaDto>>
    {}

}
