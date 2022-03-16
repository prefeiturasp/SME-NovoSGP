namespace SME.SGP.Infra
{
    public class FechamentoNotaAlunoAprovacaoDto
    {
        public string AlunoCodigo { get; set; }
        public long ComponenteCurricularId { get; set; }
        public string Nota { get; set; }
        public string ConceitoId { get; set; }
        public int? Bimestre { get; set; }
        public bool EmAprovacao { get; set; }
    }
}
