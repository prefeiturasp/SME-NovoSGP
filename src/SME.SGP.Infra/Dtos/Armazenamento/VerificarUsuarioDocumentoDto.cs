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

        public VerificarUsuarioDocumentoDto(long tipoDocumentoId, long classificacaoId, long usuarioId)
        {
            TipoDocumentoId = tipoDocumentoId;
            ClassificacaoId = classificacaoId;
            UsuarioId = usuarioId;
        }
        public long TipoDocumentoId { get; set; }
        public long ClassificacaoId { get; set; }
        public long UsuarioId { get; set; }
    }
}
