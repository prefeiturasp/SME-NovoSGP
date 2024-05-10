namespace SME.SGP.Infra
{
    public class FechamentoNotaAlunoAprovacaoDto
    {
        public string AlunoCodigo { get; set; }
        public long ComponenteCurricularId { get; set; }
        public double? Nota { get; set; }
        public double? ConceitoId { get; set; }
        public int? Bimestre { get; set; }
        public bool EmAprovacao { get; set; }
    }
}
