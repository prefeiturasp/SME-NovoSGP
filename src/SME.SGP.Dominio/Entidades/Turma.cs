namespace SME.SGP.Dominio
{
    public class Turma
    {
        public string Ano { get; set; }
        public int AnoLetivo { get; set; }
        public long Id { get; set; }
        public Modalidade Modalidade { get; set; }
        public string Nome { get; set; }
        public int QuantidadeDuracaoAula { get; set; }
        public int Semestre { get; set; }
        public int TipoTurno { get; set; }
        public string TurmaId { get; set; }
        public long UEId { get; set; }
    }
}