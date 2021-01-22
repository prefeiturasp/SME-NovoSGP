using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IListarOcorrenciasUseCase : IUseCase<FiltroOcorrenciaListagemDto, PaginacaoResultadoDto<OcorrenciaListagemDto>>
    {
    }
}
