namespace SME.SGP.Dto
{
    public class ConsolidacaoNotaAlunoDto
    {
        public string AlunoCodigo { get; set; }
        public long TurmaId { get; set; }
        public int? Bimestre { get; set; }
        public int AnoLetivo { get; set; }
        public double? Nota { get; set; }
        public long? ConceitoId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public bool Inativo { get; set; }
        public bool ConselhoClasse { get; set; }

    }
}