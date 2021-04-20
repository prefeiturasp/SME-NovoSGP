namespace SME.SGP.Dominio
{
    public class ItineranciaUe : EntidadeBase
    {
        public Ue Ue { get; set; }
        public long UeId { get; set; }
        public long ItineranciaId { get; set; }
    }
}