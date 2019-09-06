namespace SME.SGP.Dto
{
    public class NotificacaoDetalheDto
    {
        public string AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public long Id { get; set; }
        public string Mensagem { get; set; }
        public bool MostrarBotaoMarcarComoLido { get; set; }
        public bool MostrarBotoesDeAprovacao { get; set; }
        public string Situacao { get; set; }
        public string Tipo { get; set; }
        public string Titulo { get; set; }
    }
}