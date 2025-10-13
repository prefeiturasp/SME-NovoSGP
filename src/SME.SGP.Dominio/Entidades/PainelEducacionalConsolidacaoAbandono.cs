using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalConsolidacaoAbandono
    {
        public string CodigoDre { get; set; }
        public string Ano { get; set; }
        public string Modalidade { get; set; }
        public int QuantidadeDesistencias { get; set; }
        public int AnoLetivo { get; set; }
        public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;
    }
}