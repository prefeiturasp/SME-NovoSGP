using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalConsolidacaoNotaBase
    {
        public long Id { get; set; }
        public short AnoLetivo { get; set; }
        public short Bimestre { get; set; }
        public Modalidade Modalidade { get; set; }
        public string CodigoDre { get; set; }
        public int QuantidadeAbaixoMediaPortugues { get; set; }
        public int QuantidadeAcimaMediaPortugues { get; set; }
        public int QuantidadeAbaixoMediaMatematica { get; set; }
        public int QuantidadeAcimaMediaMatematica { get; set; }
        public int QuantidadeAbaixoMediaCiencias { get; set; }
        public int QuantidadeAcimaMediaCiencias { get; set; }
        public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;
    }
}