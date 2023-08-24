﻿using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class SecaoConfigRelatorioPeriodicoPAP : EntidadeBase
    {
        public int ConfiguracaoRelatorioId { get; set; }
        public ConfiguracaoRelatorioPAP ConfiguracaoRelatorio { get; set; }
        public IEnumerable<RelatorioPeriodicoPAPSecao> RelatoriosPeriodicosPAPSecao { get; set; }
    }
}
