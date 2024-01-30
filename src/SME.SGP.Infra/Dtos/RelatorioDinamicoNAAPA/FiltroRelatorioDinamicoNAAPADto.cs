using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioDinamicoNAAPADto
    {
        public bool Historico { get; set; }
        public int AnoLetivo { get; set; }
        public long? DreId { get; set; }
        public long? UeId { get; set; }
        public Modalidade[] Modalidades { get; set; }
        public List<string> Anos { get; set; }
        public List<FiltroComponenteRelatorioDinamicoNAAPA> FiltroAvancado { get; set; }
    }
}
