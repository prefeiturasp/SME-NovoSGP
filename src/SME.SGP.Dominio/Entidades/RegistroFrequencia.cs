namespace SME.SGP.Dominio
{
    public class RegistroFrequencia : EntidadeBase
    {
        public Aula Aula { get; set; }
        public long AulaId { get; set; }
        public bool Migrado { get; set; }
    }
}