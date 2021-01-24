using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IListarDocumentosUseCase
    {
        Task<PaginacaoResultadoDto<DocumentoDto>> Executar(long ueId = 0, long tipoDocumentoId = 0, long classificacaoId = 0);
    }
}