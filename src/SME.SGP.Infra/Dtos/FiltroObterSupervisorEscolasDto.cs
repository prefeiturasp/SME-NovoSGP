namespace SME.SGP.Infra
{
    public class FiltroObterSupervisorEscolasDto
    {
        public string DreCodigo { get; set; }
        public int? TipoCodigo { get; set; }
        public string UeCodigo { get; set; }
        public string SupervisorId { get; set; }
        public bool UESemResponsavel { get; set; }
    }
}
