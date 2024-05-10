namespace SME.SGP.Infra
{
    public class NotificacaoDetalheDto
    {
        public string AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public int CategoriaId { get; set; }
        public long Codigo { get; set; }
        public string CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public long Id { get; set; }
        public string Mensagem { get; set; }
        public bool MostrarBotaoMarcarComoLido { get; set; }
        public bool MostrarBotaoRemover { get; set; }
        public bool MostrarBotoesDeAprovacao { get; set; }
        public string Observacao { get; set; }
        public string Situacao { get; set; }
        public int StatusId { get; set; }
        public string Tipo { get; set; }
        public int TipoId { get; set; }
        public string Titulo { get; set; }
        public string UsuarioRf { get; set; }
    }
}