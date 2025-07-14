using System;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ConfiguracaoRelatorioPAP : EntidadeBase
    {
        private const char SEMESTRE = 'S';
        public DateTime InicioVigencia { get; set; }
        public DateTime FimVigencia { get; set; }
        public char TipoPeriocidade { get; set; }
        public bool EhSemestre { get { return TipoPeriocidade == SEMESTRE; } }
    }
}
