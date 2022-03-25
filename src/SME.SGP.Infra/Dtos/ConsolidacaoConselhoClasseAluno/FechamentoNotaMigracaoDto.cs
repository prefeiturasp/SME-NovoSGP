namespace SME.SGP.Infra
{
    public class FechamentoNotaMigracaoDto
    {
        public long DisciplinaId { get; set; }
        public double? Nota { get; set; }
        public long? ConceitoId { get; set; }
        public string AlunoCodigo { get; set; }
        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
    }
}
