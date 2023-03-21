using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IListarDocumentosUseCase
    {
        Task<PaginacaoResultadoDto<DocumentoResumidoDto>> Executar(int? anoLetivo, long ueId = 0, long tipoDocumentoId = 0, long classificacaoId = 0);
    }
}