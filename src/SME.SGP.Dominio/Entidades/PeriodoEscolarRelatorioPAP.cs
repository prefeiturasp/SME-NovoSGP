using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class PeriodoEscolarRelatorioPAP : EntidadeBase
    {
        public long PeriodoRelatorioId { get; set; }
        public PeriodoRelatorioPAP PeriodoRelatorio { get; set; }
        public long PeriodoEscolarId { get; set; }
        public PeriodoEscolar PeriodoEscolar { get; set; }
    }
}
