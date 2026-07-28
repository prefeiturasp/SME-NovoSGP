using Dapper.Contrib.Extensions;
using System;

namespace SME.SGP.Dominio
{
    public class ConfiguracaoRelatorioPAP : EntidadeBase
    {
        private const char SEMESTRE = 'S';
        public DateTime InicioVigencia { get; set; }
        public DateTime FimVigencia { get; set; }
        public char TipoPeriocidade { get; set; }
        [Write(false)]
        public bool EhSemestre { get { return TipoPeriocidade == SEMESTRE; } }
    }
}
