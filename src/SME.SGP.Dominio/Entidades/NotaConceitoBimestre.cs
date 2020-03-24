namespace SME.SGP.Dominio
{
    public class NotaConceitoBimestre : EntidadeBase
    {
        public long FechamentoTurmaDisciplinaId { get; set; }
        public FechamentoTurmaDisciplina FechamentoTurmaDisciplina { get; set; }
        public long DisciplinaId { get; set; }
        public string CodigoAluno { get; set; }
        public double? Nota { get; set; }
        public long? ConceitoId { get; set; }
        public Conceito Conceito { get; set; }
        public long? SinteseId { get; set; }
        public Conceito Sintese { get; set; }
        public bool Migrado { get; set; }
        public bool Excluido { get; set; }
    }
}