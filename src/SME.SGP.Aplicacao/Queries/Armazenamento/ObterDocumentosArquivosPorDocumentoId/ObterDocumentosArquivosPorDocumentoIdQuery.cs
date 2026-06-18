using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDocumentosArquivosPorDocumentoIdQuery : IRequest<IEnumerable<DocumentoArquivoDto>>
    {
        public ObterDocumentosArquivosPorDocumentoIdQuery(long documentoId)
        {
            DocumentoId = documentoId;
        }

        public long DocumentoId { get; }
    }
}