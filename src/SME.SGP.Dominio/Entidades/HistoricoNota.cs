namespace SME.SGP.Dominio
{
    public class HistoricoNota : EntidadeBase
    {
        public long Id { get; set; }
        public string NotaAnterior { get; set; }
        public string NotaNova { get; set; }
    }
}
