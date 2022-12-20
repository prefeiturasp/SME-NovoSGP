using System;

namespace SME.SGP.Dominio
{
    public class Documento : EntidadeBase
    {
        public long ClassificacaoDocumentoId { get; set; }
        public ClassificacaoDocumento ClassificacaoDocumento { get; set; }
        public long UsuarioId { get; set; }
        public long UeId { get; set; }

        public long AnoLetivo { get; set; }
        public long? TurmaId { get; set; }
        public long? ComponenteCurricularId { get; set; }

    }
}