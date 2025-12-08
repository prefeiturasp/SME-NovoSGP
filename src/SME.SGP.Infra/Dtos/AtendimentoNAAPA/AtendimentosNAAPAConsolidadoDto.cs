using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra
{
    public class AtendimentosNAAPAConsolidadoDto
    {
        public long UeId { get; set; }
        public int AnoLetivo { get; set; }
        public SituacaoNAAPA Situacao { get; set; }
        public long Quantidade { get; set; }
        public Modalidade Modalidade { get; set; }
    }
}