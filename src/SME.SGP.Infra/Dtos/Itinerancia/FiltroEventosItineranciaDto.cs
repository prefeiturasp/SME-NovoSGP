namespace SME.SGP.Infra
{
    public class FiltroEventosItineranciaDto
    {
        public FiltroEventosItineranciaDto(long tipoCalendarioId, long itineranciaId, string codigoUE)
        {
            TipoCalendarioId = tipoCalendarioId;
            ItineranciaId = itineranciaId;
            CodigoUE = codigoUE;
        }

        public long TipoCalendarioId { get; set; }
        public long ItineranciaId { get; set; }
        public string CodigoUE { get; set; }
    }
}
