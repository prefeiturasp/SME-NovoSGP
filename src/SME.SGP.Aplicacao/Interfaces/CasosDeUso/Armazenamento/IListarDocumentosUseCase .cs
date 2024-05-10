using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IListarDocumentosUseCase
    {
        Task<PaginacaoResultadoDto<DocumentoResumidoDto>> Executar(FiltroListagemDocumentosDto filtro);
    }
}