using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDocumentoArquivo
    {
        Task<long> SalvarAsync(DocumentoArquivo documentoArquivo);
        Task<IEnumerable<DocumentoArquivoDto>> ObterDocumentosArquivosPorDocumentoIdAsync(long documentoId);
    }
}