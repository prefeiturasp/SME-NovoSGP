using System;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalIdebDto
    {
        public int AnoLetivo { get; set; }
        public int SerieAno { get; set; }
        public decimal Nota { get; set; }
        public string Faixa { get; set; }
        public DateTime CriadoEm { get; set; }
        public int CodigoDre { get; set; }
        public int CodigoUe { get; set; }
        public int Quantidade { get; set; }
    }
}
