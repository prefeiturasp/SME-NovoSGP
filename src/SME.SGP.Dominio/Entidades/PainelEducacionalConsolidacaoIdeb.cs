using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalConsolidacaoIdeb
    {
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public PainelEducacionalIdebSerie Etapa { get; set; }
        public string Faixa { get; set; }
        public int Quantidade { get; set; }
        public decimal MediaGeral { get; set; }

        public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;
    }
}
