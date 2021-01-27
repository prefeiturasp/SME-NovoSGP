using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class SalvarDocumentoDto
    {
        public SalvarDocumentoDto() { }

        public SalvarDocumentoDto(Guid arquivoCodigo, long ueId, long tipoDocumentoId, long classificacaoId, long usuarioId)
        {
            ArquivoCodigo = arquivoCodigo;
            UeId = ueId;
            TipoDocumentoId = tipoDocumentoId;
            ClassificacaoId = classificacaoId;
            UsuarioId = usuarioId;
        }

        public long UeId { get; set; }
        public long AnoLetivo { get; set; }
        public long TipoDocumentoId { get; set; }
        public long ClassificacaoId { get; set; }
        public long UsuarioId { get; set; }
        public Guid ArquivoCodigo { get; set; }
    }
}
