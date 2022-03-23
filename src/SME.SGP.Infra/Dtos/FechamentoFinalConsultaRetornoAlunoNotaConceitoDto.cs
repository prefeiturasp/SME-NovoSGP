namespace SME.SGP.Infra
{
    public class FechamentoFinalConsultaRetornoAlunoNotaConceitoDto
    {
        public int Bimestre { get; set; }
        public string Disciplina { get; set; }
        public long DisciplinaCodigo { get; set; }
        public string NotaConceito { get; set; }
        public bool EmAprovacao { get; set; }
    }
}