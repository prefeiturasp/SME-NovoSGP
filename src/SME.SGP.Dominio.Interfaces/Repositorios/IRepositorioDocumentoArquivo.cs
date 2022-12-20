using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDocumentoArquivo
    {
        Task<long> SalvarAsync(DocumentoArquivo documentoArquivo);
        Task<IEnumerable<DocumentoArquivo>> ObterDocumentosArquivosPorDocumentoIdAsync(long documentoId);
    }
}