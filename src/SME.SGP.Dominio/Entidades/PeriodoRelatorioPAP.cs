using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class PeriodoRelatorioPAP : EntidadeBase
    {
        public long ConfiguracaoId { get; set; }
        public ConfiguracaoRelatorioPAP Configuracao { get; set; }
        public int Periodo { get; set; }
        public List<PeriodoEscolarRelatorioPAP> PeriodosEscolaresRelatorio { get; set; }
    }
}
