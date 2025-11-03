using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoPlanoAEE
{
    public class ConsolidacaoPlanoAEEDto
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int AnoLetivo { get; set; }
        public SituacaoPlanoAEE SituacaoPlano { get; set; }
        public int QuantidadeSituacaoPlano { get; set; }
    }
}
