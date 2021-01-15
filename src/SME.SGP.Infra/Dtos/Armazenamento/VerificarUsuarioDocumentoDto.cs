using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class VerificarUsuarioDocumentoDto
    {
        public VerificarUsuarioDocumentoDto()
        {

        }

        public VerificarUsuarioDocumentoDto(long tipoDocumentoId, long classificacaoId, long usuarioId, long ueId, long documentoId)
        {
            TipoDocumentoId = tipoDocumentoId;
            ClassificacaoId = classificacaoId;
            UsuarioId = usuarioId;
            UeId = ueId;
            DocumentoId = documentoId;
        }
        public long DocumentoId { get; set; }
        public long TipoDocumentoId { get; set; }
        public long ClassificacaoId { get; set; }
        public long UsuarioId { get; set; }
        public long UeId { get; set; }
    }
}
