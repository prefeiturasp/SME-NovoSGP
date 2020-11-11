using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class DocumentoDto
    {
        public DocumentoDto() { }

        public DocumentoDto(long tipoDocumentoId, long classificacaoId, long usuarioId, string usuario, DateTime dataUpload)
        {
            TipoDocumentoId = tipoDocumentoId;
            ClassificacaoId = classificacaoId;
            UsuarioId = usuarioId;
            Usuario = usuario;
            DataUpload = dataUpload;
        }

        public string Usuario { get; set; }
        public DateTime DataUpload { get; set; }
        public long TipoDocumentoId { get; set; }
        public long ClassificacaoId { get; set; }
        public long UsuarioId { get; set; }
    }
}
