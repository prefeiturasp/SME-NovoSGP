using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class PeriodoRelatorioPAP : EntidadeBase
    {
        public long ConfiguracaoId { get; set; }
        public ConfiguracaoRelatorioPAP Configuracao { get; set; }
        public int Periodo { get; set; }
        public List<PeriodoEscolarRelatorioPAP> PeriodosEscolaresRelatorio { get; set; }
    }
}
