namespace SME.SGP.Dominio
{
    public class PendenciaAula
    {
        public PendenciaAula()
        {

        }
        public PendenciaAula(long aulaId)
        {
            AulaId = aulaId;
        }
        public long Id { get; set; }
        public long AulaId { get; set; }
        public long PendenciaId { get; set; }
        public string Motivo { get; set; }        
    }
}