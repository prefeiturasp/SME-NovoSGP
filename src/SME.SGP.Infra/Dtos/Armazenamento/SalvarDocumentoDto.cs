using System;

namespace SME.SGP.Infra
{
    public class SalvarDocumentoDto
    {
        public SalvarDocumentoDto() { }

        public SalvarDocumentoDto(Guid[] arquivosCodigos, long ueId, long tipoDocumentoId, long classificacaoId, long usuarioId)
        {
            ArquivosCodigos = arquivosCodigos;
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
        public Guid[] ArquivosCodigos { get; set; }
        public long? TurmaId { get; set; }
        public long? ComponenteCurricularId { get; set; }
    }
}
