using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class NotificacaoFiltroDto
    {
        public int Ano { get; set; }
        public int AnoLetivo { get; set; }
        public NotificacaoCategoria Categoria { get; set; }
        public long Codigo { get; set; }
        public string DreId { get; set; }
        public NotificacaoStatus Status { get; set; }
        public NotificacaoTipo Tipo { get; set; }
        public string Titulo { get; set; }
        public string TurmaId { get; set; }
        public string UeId { get; set; }
        public string UsuarioRf { get; set; }
    }
}