using System;

namespace SME.SGP.Dominio
{
    public class ConfiguracaoRelatorioPAP : EntidadeBase
    {
        public DateTime InicioVigencia { get; set; }
        public DateTime FimVigencia { get; set; }
        public char TipoPeriocidade { get; set; }
    }
}
