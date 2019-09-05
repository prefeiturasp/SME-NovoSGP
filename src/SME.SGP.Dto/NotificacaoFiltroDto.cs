using SME.SGP.Dominio;

namespace SME.SGP.Dto
{
    public class NotificacaoFiltroDto
    {
        public string DreId { get; set; }
        public string EscolaId { get; set; }
        public NotificacaoStatus Status { get; set; }
        public NotificacaoTipo Tipo { get; set; }
        public string TurmaId { get; set; }
        public string UsuarioId { get; set; }
        public NotificacaoCategoria Categoria { get; set; }
    }
}