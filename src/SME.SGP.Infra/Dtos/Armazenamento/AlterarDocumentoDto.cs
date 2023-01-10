using System;

namespace SME.SGP.Infra
{
    public class AlterarDocumentoDto
    {
        public AlterarDocumentoDto() { }

        public AlterarDocumentoDto(long documentoId, Guid[] arquivosCodigos)
        {
            DocumentoId = documentoId;
            ArquivosCodigos = arquivosCodigos;
        }

        public long DocumentoId { get; set; }
        public Guid[] ArquivosCodigos { get; set; }
    }
}
