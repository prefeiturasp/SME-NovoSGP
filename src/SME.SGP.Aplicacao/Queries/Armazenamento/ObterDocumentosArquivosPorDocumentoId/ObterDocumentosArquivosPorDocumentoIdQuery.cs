using System.Collections.Generic;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;

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