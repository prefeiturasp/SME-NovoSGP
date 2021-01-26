using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AlterarDocumentoDto
    {
        public AlterarDocumentoDto() { }

        public AlterarDocumentoDto(long documentoId, Guid codigoArquivo)
        {
            DocumentoId = documentoId;
            CodigoArquivo = codigoArquivo;
        }

        public long DocumentoId { get; set; }
        public Guid CodigoArquivo { get; set; }
    }
}
