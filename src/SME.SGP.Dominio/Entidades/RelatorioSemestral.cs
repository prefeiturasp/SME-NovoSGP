namespace SME.SGP.Dominio
{
    public class RelatorioSemestral
    {
        public long Id { get; set; }
        public long TurmaId { get; set; }
        public Turma Turma { get; set; }
        public int Semestre { get; set; }
    }
}