using System;

namespace SME.SGP.Dominio
{
    public class EncaminhamentoNAAPAHistoricoAlteracoes
    {
        public long Id { get; set; }
        public long EncaminhamentoNAAPAId { get; set; }
        public long? SecaoEncaminhamentoNAAPAId { get; set; }
        public long UsuarioId { get; set; }
        public EncaminhamentoNAAPASecao EncaminhamentoNAAPASecao { get; set; }
        public Usuario Usuario { get; set; }
        public string CamposInseridos { get; set; }
        public string CamposAlterados { get; set; }
        public string DataAtendimento { get; set; }
        public DateTime DataHistorico { get; set; }
        public TipoHistoricoAlteracoesEncaminhamentoNAAPA TipoHistorico { get; set; }
    }
}
