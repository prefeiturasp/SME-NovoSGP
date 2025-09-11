namespace SME.SGP.Dominio
{
    public class PeriodoEscolarRelatorioPAP : EntidadeBase
    {
        public long PeriodoRelatorioId { get; set; }
        public PeriodoRelatorioPAP PeriodoRelatorio { get; set; }
        public long PeriodoEscolarId { get; set; }
        public PeriodoEscolar PeriodoEscolar { get; set; }
    }
}
