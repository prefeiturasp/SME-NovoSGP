using System.Collections.Generic;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterDocumentosArquivosPorDocumentoIdQuery : IRequest<IEnumerable<DocumentoArquivo>>
    {
        public ObterDocumentosArquivosPorDocumentoIdQuery(long documentoId)
        {
            DocumentoId = documentoId;
        }

        public long DocumentoId { get; }
    }
}