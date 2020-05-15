namespace SME.SGP.Dominio
{
    public class RelatorioSemestralTurmaPAP
    {
        public long Id { get; set; }
        public long TurmaId { get; set; }
        public Turma Turma { get; set; }
        public int Semestre { get; set; }
    }
}