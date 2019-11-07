namespace SME.SGP.Infra
{
    public class CalendarioEventosFiltroDto
    {
        public string DreId { get; set; }
        public bool EhEventoSme { get; set; }
        public long IdTipoCalendario { get; set; }
        public string UeId { get; set; }
    }
}