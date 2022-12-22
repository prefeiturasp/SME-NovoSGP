using System;

namespace SME.SGP.Infra.Dtos
{
    public class DocumentoArquivoDto
    {
        public long Id { get; set; }
        public long DocumentoId { get; set; }
        public long ArquivoId { get; set; }
        public Guid Codigo { get; set; }
    }
}
