using System;

namespace SME.SGP.Dominio
{
    public class EncaminhamentoNAAPAAuditoria
    {
        public long Id { get; set; }
        public long EncaminhamentoNAAPAId { get; set; }
        public long EncaminhamentoNAAPASecaoId { get; set; }
        public long UsuarioId { get; set; }
        public EncaminhamentoNAAPASecao EncaminhamentoNAAPASecao { get; set; }
        public Usuario Usuario { get; set; }
        public string CamposInseridos { get; set; }
        public string CamposAlterados { get; set; }
        public DateTime DataAuditoria { get; set; }
        public TipoAuditoriaEncaminhamentoNAAPA TipoAuditoria { get; set; }
    }
}
