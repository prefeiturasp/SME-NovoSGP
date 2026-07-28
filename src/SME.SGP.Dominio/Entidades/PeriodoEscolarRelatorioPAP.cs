using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class PeriodoEscolarRelatorioPAP : EntidadeBase
    {
        public long PeriodoRelatorioId { get; set; }
        [Computed]
        public PeriodoRelatorioPAP PeriodoRelatorio { get; set; }
        public long PeriodoEscolarId { get; set; }
        [Computed]
        public PeriodoEscolar PeriodoEscolar { get; set; }
    }
}
