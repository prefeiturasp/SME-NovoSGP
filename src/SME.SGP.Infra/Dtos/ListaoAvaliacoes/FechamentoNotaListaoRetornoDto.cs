namespace SME.SGP.Infra
{
    public class FechamentoNotaListaoRetornoDto
    {
        public string ConceitoDescricao { get; set; }
        public string Disciplina { get; set; }
        public long DisciplinaId { get; set; }
        public bool EhConceito { get; set; }
        public double? NotaConceito { get; set; }
        public bool EmAprovacao { get; set; }
    }
}
