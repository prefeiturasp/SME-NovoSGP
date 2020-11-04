namespace SME.SGP.Dominio
{
    public class HistoricoNota : EntidadeBase
    {
        public double? NotaAnterior { get; set; }
        public double? NotaNova { get; set; }
        public long? ConceitoAnteriorId { get; set; }
        public Conceito ConceitoAnterior { get; set; }
        public long? ConceitoNovoId { get; set; }
        public Conceito ConceitoNovo { get; set; }
    }
}
