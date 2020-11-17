namespace SME.SGP.Infra.Dtos
{
    public class NotificacoesParaTratamentoCargosNiveisDto
    {
        public int Cargo { get; set; }
        public string NotificacaoTitulo { get; set; }
        public string NotificacaoMensagem { get; set; }
        public int NotificacaoStatus { get; set; }
        public int NotificacaoCategoria { get; set; }
        public int NotificacaoTipo { get; set; }
        public int UEId { get; set; }
        public int DREId { get; set; }
        public int NotificacaoAnoLetivo { get; set; }
        public long NotificacaoCodigo { get; set; }
        public long UsuarioId { get; set; }
        public string UsuarioRF { get; set; }

    }
}
