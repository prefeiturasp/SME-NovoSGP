namespace SME.SGP.Dominio
{
    public class DiarioBordo: EntidadeBase
    {
        public long AulaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long TurmaId { get; set; }
        public Aula Aula { get; set; }
        public long? DevolutivaId { get; set; }
        public Devolutiva Devolutiva { get; set; }

        public string Planejamento { get; set; }

        public bool Excluido { get; set; }
        public bool Migrado { get; set; }
        public bool InseridoCJ { get; set; }


        public void AdicionarAula(Aula aula)
        {
            Aula = aula ?? throw new NegocioException("É necessario informar uma aula.");
        }
    }

    
}
