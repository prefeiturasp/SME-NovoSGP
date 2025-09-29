using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalIdepAgrupamento
    {
        public int AnoLetivo { get; set; }
        public string Etapa { get; set; }
        public decimal Nota { get; set; }
        public DateTime CriadoEm { get; set; }
        public int CodigoDre { get; set; }
        public int CodigoUe { get; set; }
    }
}
