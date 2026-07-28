using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class PeriodoRelatorioPAP : EntidadeBase
    {
        public long ConfiguracaoId { get; set; }
        [Computed]
        public ConfiguracaoRelatorioPAP Configuracao { get; set; }
        public int Periodo { get; set; }
        [Computed]
        public List<PeriodoEscolarRelatorioPAP> PeriodosEscolaresRelatorio { get; set; }
    }
}
