using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalConsolidacaoPlanoAEE
    {
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string SituacaoPlano { get; set; }
        public int QuantidadeSituacaoPlano { get; set; }
        public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;
    }
}