namespace SME.SGP.Dominio
{
    public class PendenciaAula
    {
        public PendenciaAula()
        {

        }
        public PendenciaAula(long aulaId, TipoPendenciaAula tipoPendenciaAula)
        {
            AulaId = aulaId;
            TipoPendenciaAula = tipoPendenciaAula;
        }
        public long Id { get; set; }
        public long AulaId { get; set; }
        public TipoPendenciaAula TipoPendenciaAula { get; set; }
    }
}