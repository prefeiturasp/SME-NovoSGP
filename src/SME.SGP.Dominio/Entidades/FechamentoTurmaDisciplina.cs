namespace SME.SGP.Dominio
{
    public class FechamentoTurmaDisciplina : EntidadeBase
    {
        public long PeriodoFechamentoBimestreId { get; set; }
        public PeriodoFechamentoBimestre PeriodoFechamentoBimestre { get; set; }
        public long TurmaId { get; set; }
        public Turma Turma { get; set; }
        public long DisciplinaId { get; set; }

        public bool Migrado { get; set; }
        public bool Excluido { get; set; }
    }
}