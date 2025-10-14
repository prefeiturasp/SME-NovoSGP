using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalIdepDto
    {
        public int AnoLetivo { get; set; }
        public PainelEducacionalIdepEtapa Etapa { get; set; }
        public string Faixa { get; set; }
        public int Quantidade { get; set; }
        public decimal MediaGeral { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
        public string CodigoDre { get; set; }
    }
}
